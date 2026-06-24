using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.User
{
    public class GoogleLoginViewModel
    {
        public required GoogleAuthViewModel GoogleAuth { get; set; }
    }
    public class GoogleAuthViewModel
    {
        public required string AuthorizationCode { get; set; }
        public required string RedirectUri { get; set; }
    }
}
