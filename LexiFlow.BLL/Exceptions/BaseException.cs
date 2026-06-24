using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Exceptions
{
    public abstract class BaseException : Exception
    {
        protected BaseException(string title, string message)
            : base(message) =>
            Title = title;

        public string Title { get; }
        public int StatusCode { get; set; }
    }
}
