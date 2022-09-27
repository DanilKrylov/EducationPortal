using EducationPortal.Application.Extensions;
using EducationPortal.Application.Interfaces;
using EducationPortal.Application.Validators;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> EmailIsUniqueAsync(string email)
        {
            var users = GetUsers();
            var user = await users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public IQueryable<User> GetUsers()
        {
            return _userManager.Users;
        }

        public async Task<bool> LoginIsUniqueAsync(string login)
        {
            var users = GetUsers();
            var user = await users.FirstOrDefaultAsync(x => x.UserName == login);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> PhoneIsUniqueAsync(string phone)
        {
            var users = GetUsers();
            var user = await users.FirstOrDefaultAsync(x => x.PhoneNumber == phone);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<CommandResult<Dictionary<string, string>>> RegisterAsync(User user, string password)
        {
            var validationResult = await new UserRegisterValidator(this).ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                return new CommandResult<Dictionary<string, string>>()
                {
                    Success = false,
                    ErrorMessage = validationResult.Errors.ToDictionary()
                };
            }

            await _userManager.CreateAsync(user, password);
            await _signInManager.SignInAsync(user, false);

            return new CommandResult<Dictionary<string, string>>() { Success = true };
        }

        public async Task<CommandResult<string>> LoginAsync(string login, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(login, password, true, false);
            if (result.Succeeded)
            {
                return new CommandResult<string> { Success = true };
            }

            return new CommandResult<string>
            {
                Success = false,
                ErrorMessage = "Login or password entered incorrectly"
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
