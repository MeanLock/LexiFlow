using Google.Apis.Auth;
using LexiFlow.BLL.Exceptions;
using LexiFlow.BLL.Models;
using LexiFlow.BLL.Models.Emails;
using LexiFlow.BLL.Models.User;
using LexiFlow.DAL.Entities;
using LexiFlow.DAL.Repositories;
using LexiFlow.DAL.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<EmailVerification> _emailVerificationRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IOptions<JwtOption> _jwtOption;

        public UserService(
            IGenericRepository<User> userRepository,
            IGenericRepository<EmailVerification> emailVerificationRepository,
            IOptions<JwtOption> jwtOption,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            IConfiguration config)
        {
            _config = config;
            _userRepository = userRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _jwtOption = jwtOption;
        }

        public async Task<AuthorizedResponseModel> Login(LoginViewModel model)
        {
            var user = await _userRepository.FindSingleAsync(a => a.Email == model.Email);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var checkPassword = VerifyPassword(user.Password, model.Password);
            if (!checkPassword)
            {
                throw new UserException.LoginNotCorrectException();
            }

            if (!user.IsVerified)
                throw new UserException.UnverifyException();

            List<Claim> claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role == 1 ? "Learner" : "Admin"),
                new Claim("UserId", user.Id.ToString())

            };
            var response = new AuthorizedResponseModel() { JwtToken = GenerateAccessToken(claims) };
            return response;
        }

        public async Task<AuthorizedResponseModel> LoginGoogle(GoogleLoginViewModel model)
        {
            string authorizationCode = model.GoogleAuth.AuthorizationCode;
            string redirectUri = model.GoogleAuth.RedirectUri;
            var clientId = _config["GoogleOAuth2Config:ClientId"];
            var clientSecret = _config["GoogleOAuth2Config:ClientSecret"];

            using var httpClient = new HttpClient();
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token");
            var tokenParams = new Dictionary<string, string>
            {
                { "code", authorizationCode },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            };
            tokenRequest.Content = new FormUrlEncodedContent(tokenParams);
            var responseGoogle = await httpClient.SendAsync(tokenRequest);
            if (!responseGoogle.IsSuccessStatusCode)
            {
                var errorContent = await responseGoogle.Content.ReadAsStringAsync();
                throw new UserException.UnauthorizedGoogleException();
            }
            var payloadStr = await responseGoogle.Content.ReadAsStringAsync();
            var payload = JObject.Parse(payloadStr);
            string idToken = payload["id_token"]?.ToString();
            if (string.IsNullOrEmpty(idToken))
                throw new UnauthorizedAccessException("ID Token missing in response");

            var googlePayload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            });

            if (googlePayload == null)
                throw new UnauthorizedAccessException("Invalid ID Token");

            var user = await SignInOrCreateGoogleUser(googlePayload);

            List<Claim> claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role == 1 ? "Learner" : "Admin"),
                new Claim("UserId", user.Id.ToString())

            };

            var response = new AuthorizedResponseModel() { JwtToken = GenerateAccessToken(claims) };
            return response;
        }

        public async Task<ResponseResult> Register(RegisterViewModel model)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existingUser = await _userRepository
                    .FindSingleAsync(x => x.Email == model.Email);

                if (existingUser != null)
                    throw new UserException.RegisterException();

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = model.FullName,
                    Dob = model.Dob,
                    Email = model.Email,
                    Password = Hash(model.Password),
                    Role = 1,
                    IsVerified = false,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                _userRepository.Add(user);

                var otp = GenerateOtp();

                var emailVerification = new EmailVerification
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    Otp = Hash(otp),
                    CreatedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(10)
                };

                _emailVerificationRepository.Add(emailVerification);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                await _emailService.SendOtpAsync(model.Email, otp);

                return ResponseResult.Success("Register successful. Please verify OTP.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<ResponseResult> VerifyOtpAsync(VerifyOtpRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Get latest OTP 
                var otpRecord = await _emailVerificationRepository
                    .FindSingleAsync(x =>
                        x.Email == request.Email &&
                        x.VerifiedAt == null);

                if (otpRecord == null)
                    throw new Exception("OTP not found");

                // 2. Check expired
                if (otpRecord.ExpiredAt < DateTime.UtcNow)
                    throw new Exception("OTP expired");

                // 3. Compare OTP 
                var hashedInputOtp = Hash(request.Otp);

                if (otpRecord.Otp != hashedInputOtp)
                    throw new Exception("Invalid OTP");

                // 4. Mark OTP as verified
                otpRecord.VerifiedAt = DateTime.UtcNow;

                _emailVerificationRepository.Update(otpRecord);

                // 5. Get user
                var user = await _userRepository
                    .FindSingleAsync(x => x.Email == request.Email);

                if (user == null)
                    throw new Exception("User not found");

                // 6. Activate user
                user.IsVerified = true;
                user.UpdatedAt = DateTime.UtcNow;

                _userRepository.Update(user);

                // 7. Save
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return ResponseResult.Success("OTP verified successfully. User activated.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }

        private string Hash(string message)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
                return Convert.ToBase64String(bytes);
            }
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            var newHash = Hash(password);
            return newHash.Equals(hashedPassword);
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtOption.Value.Issuer,
                audience: _jwtOption.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOption.Value.ExpireMin),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        private async Task<User?> SignInOrCreateGoogleUser(GoogleJsonWebSignature.Payload googlePayload)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var user = await _userRepository.FindSingleAsync(a => a.Email == googlePayload.Email);
                if (user == null)
                {
                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = googlePayload.Email,
                        Password = Hash(Guid.NewGuid().ToString()),
                        FullName = googlePayload.Name ?? googlePayload.Email,
                        GoogleId = googlePayload.Subject,
                        Dob = new DateOnly(2000, 1, 1),// chỗ này cần sửa lại cho phép Dob null vì gg không trả về Dob
                        Role = 1,
                        IsVerified = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow
                    };
                    _userRepository.Add(newUser);
                    await _unitOfWork.SaveChangesAsync();

                    user = newUser;
                }
                else
                {
                    if (string.IsNullOrEmpty(user.GoogleId))
                    {
                        if (!googlePayload.EmailVerified)
                            throw new UnauthorizedAccessException("Google email not verified");

                        user.GoogleId = googlePayload.Subject;

                        if (!user.IsVerified)
                            user.IsVerified = true;

                        user.UpdatedAt = DateTime.UtcNow;

                        _userRepository.Update(user);
                        await _unitOfWork.SaveChangesAsync();
                    }
                    else if (user.GoogleId != googlePayload.Subject)
                    {
                        throw new HttpRequestException("User linked to another Google");
                    }
                }

                await _unitOfWork.CommitAsync();
                return user;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                Console.WriteLine($"\n{ex.Message}\n{ex.StackTrace}\n");

                if (ex is InvalidOperationException)
                    throw new HttpRequestException(ex.Message, null, System.Net.HttpStatusCode.Conflict);

                throw new HttpRequestException("Google sign-in failed", null, System.Net.HttpStatusCode.Unauthorized);
            }
        }

    }
}
