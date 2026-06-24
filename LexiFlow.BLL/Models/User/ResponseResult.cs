using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.User
{
    public class ResponseResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public static ResponseResult Success(string message = null)
        {
            return new ResponseResult(StatusCodes.Status200OK, message);
        }

        protected ResponseResult(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }


    }

    public class ResponseResult<T> : ResponseResult
    {
        public static ResponseResult<T> Success(T data, string message = null)
        {
            return new ResponseResult<T>(StatusCodes.Status200OK, message, data);
        }

        protected ResponseResult(int statusCode, string message, T data) : base(statusCode, message)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
