using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Exceptions
{
    public static class UserException
    {
        public class UserNotFoundException : NotFoundException
        {
            public UserNotFoundException()
                : base($"User not found") { }
        }

        public class LoginNotCorrectException : NotFoundException
        {
            public LoginNotCorrectException()
                : base($"Email or Password is not correct") { }
        }
        public class UnverifyException : BadRequestException
        {
            public UnverifyException()
                : base($"This account doest not verified!!!") { }
        }
        public class UnauthorizedException : BaseException
        {
            public UnauthorizedException()
                : base("Unauthorized", "you are not authorized")
            {
                StatusCode = StatusCodes.Status401Unauthorized;
            }
        }
        public class UnauthorizedGoogleException : BaseException
        {
            public UnauthorizedGoogleException()
                : base("Unauthorized", "Error exchanging authorization code for tokens")
            {
                StatusCode = StatusCodes.Status401Unauthorized;
            }
        }

        public class RegisterException : BadRequestException
        {
            public RegisterException()
                : base($"This email is already exist") { }
        }


        public class ForbiddenException : BaseException
        {
            public ForbiddenException()
                : base("Forbidden", "you are not allowed to access this resource")
            {
                StatusCode = StatusCodes.Status403Forbidden;
            }
        }

        public class HandleUserException : BadRequestException
        {
            public HandleUserException(string message)
                : base(message) { }
        }
    }
}
