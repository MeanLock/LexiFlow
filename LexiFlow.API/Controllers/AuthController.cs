using LexiFlow.BLL.Models.Emails;
using LexiFlow.BLL.Models.User;
using LexiFlow.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LexiFlow.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _config;
        public AuthController(IUserService systemUserService, IConfiguration config)
        {
            _userService = systemUserService;
            _config = config;
        }

        [HttpPost]
        [Route("login")]
        public async Task<AuthorizedResponseModel> Login([FromBody] LoginViewModel model)
        {
            var result = await _userService.Login(model);
            return result;
        }

        [HttpPost]
        [Route("login-google")]
        public async Task<AuthorizedResponseModel> LoginGoogle([FromBody] GoogleLoginViewModel model)
        {
            var result = await _userService.LoginGoogle(model);
            return result;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ResponseResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await _userService.Register(model);
            return result;
        }

        [HttpPost]
        [Route("verify-otp")]
        public async Task<ResponseResult> VerifyOTP([FromBody] VerifyOtpRequest model)
        {
            var result = await _userService.VerifyOtpAsync(model);
            return result;
        }
    }
}
