using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Exceptions
{
    public abstract class NotFoundException : BaseException
    {
        protected NotFoundException(string message)
            : base("Not Found", message)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}
