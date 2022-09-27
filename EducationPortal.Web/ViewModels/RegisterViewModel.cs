using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class RegisterViewModel
    {
        [StringLength(12, MinimumLength = 5, ErrorMessage ="login length must be between 5 and 12")]
        public string Login { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int Age { get;set; }

        [StringLength(12, MinimumLength = 5, ErrorMessage ="password length must be between 5 and 12")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="this field must be equal to password")]
        public string ConfirmPassword { get; set; }
    }
}
