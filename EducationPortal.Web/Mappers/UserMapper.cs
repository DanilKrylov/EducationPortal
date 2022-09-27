using EducationPortal.Domain.Models;
using EducationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.Mappers
{
    internal class UserMapper
    {
        public static User ToModel(RegisterViewModel regViewModel)
        {
            return new User()
            {
                Email = regViewModel.Email,
                UserName = regViewModel.Login,
                PhoneNumber = regViewModel.PhoneNumber,
                Age = regViewModel.Age,
            };
        }
    }
}
