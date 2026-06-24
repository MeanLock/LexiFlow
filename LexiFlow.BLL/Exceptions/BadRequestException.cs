using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Exceptions
{
    public abstract class BadRequestException : BaseException
    {
        protected BadRequestException(string message)
            : base("Bad Request", message)
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
