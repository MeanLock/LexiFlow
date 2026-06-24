using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.User
{
    public class RegisterViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string FullName { get; set; }

        public DateOnly Dob { get; set; }

        public string Password { get; set; }
    }
}
