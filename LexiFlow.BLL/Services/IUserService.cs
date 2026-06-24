using LexiFlow.BLL.Models.Emails;
using LexiFlow.BLL.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public interface IUserService
    {
        Task<AuthorizedResponseModel> Login(LoginViewModel model);
        Task<AuthorizedResponseModel> LoginGoogle(GoogleLoginViewModel model);
        Task<ResponseResult> Register(RegisterViewModel model);
        Task<ResponseResult> VerifyOtpAsync(VerifyOtpRequest request);
    }
}
