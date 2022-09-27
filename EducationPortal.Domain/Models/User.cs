
using Microsoft.AspNetCore.Identity;

namespace EducationPortal.Domain.Models
{
    public class User : IdentityUser
    {
        public int Age { get; set; }
    }
}
