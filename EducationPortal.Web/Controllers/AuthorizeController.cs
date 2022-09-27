using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Models;
using EducationPortal.Web.Mappers;
using EducationPortal.Web.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationPortal.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IUserService _userService;

        public AuthorizeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserMapper.ToModel(model);
                var registerResult = await _userService.RegisterAsync(user, model.Password);
                if (registerResult.Success)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var err in registerResult.ErrorMessage)
                {
                    ModelState.AddModelError(err.Key, err.Value);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var loginResult = await _userService.LoginAsync(model.Login, model.Password);
            if (loginResult.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Password", loginResult.ErrorMessage);
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
