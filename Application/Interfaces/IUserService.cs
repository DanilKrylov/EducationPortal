using EducationPortal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> EmailIsUniqueAsync(string email);

        Task<bool> LoginIsUniqueAsync(string login);

        Task<bool> PhoneIsUniqueAsync(string phone);

        Task<CommandResult<Dictionary<string, string>>> RegisterAsync(User user, string password);

        Task<CommandResult<string>> LoginAsync(string login, string password);

        Task LogoutAsync();
    }
}
