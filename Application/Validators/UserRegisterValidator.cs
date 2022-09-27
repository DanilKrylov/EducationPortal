using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Models;
using FluentValidation;

namespace EducationPortal.Application.Validators
{
    public class UserRegisterValidator : AbstractValidator<User>
    {
        public UserRegisterValidator(IUserService userService)
        {
            RuleFor(user => user.UserName).MustAsync(async (login, _) => await userService.LoginIsUniqueAsync(login)).WithMessage("your login is allready used by some user");
            RuleFor(user => user.Email).MustAsync(async (email, _) => await userService.EmailIsUniqueAsync(email)).WithMessage("your email is allready used by some user");
            RuleFor(user => user.PhoneNumber).MustAsync(async (phone, _) => await userService.PhoneIsUniqueAsync(phone)).WithMessage("your phone is allready used by some user");
        }
    }
}
