using EducationPortal.ConsoleView.Mappers;
using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Mappers
{
    internal class UserMapper : IMapper<User>
    {
        public User ToModel(string userInput)
        {
            var userProps = userInput.Split('-');
            if (userProps.Length == 2)
            {
                return new User()
                {
                    Login = userProps[0],
                    Password = userProps[1],
                };
            }
            else if (userProps.Length == 5)
            {
                try
                {
                    return new User(userProps[0], userProps[2], Convert.ToInt32(userProps[4]), userProps[3], userProps[1]);
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Error age Format");
                }
            }

            throw new ArgumentException("Format error");
        }
    }
}
