using EducationPortal.Application.Interfaces;
using EducationPortal.Application.Managers;
using EducationPortal.Application.Validators;
using EducationPortal.ConsoleView.Mappers;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Controllers
{
    public class UserController
    {
        private readonly IUserService _userManager;
        private readonly ISignService _signManager;
        private readonly ISkillService _skillManager;

        public UserController(IUserService userManager, ISignService signManager, ISkillService skillManager)
        {
            _userManager = userManager;
            _signManager = signManager;
            _skillManager = skillManager;
        }

        public void LogOut()
        {
            try
            {
                _signManager.SignOut();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Login(string param)
        {
            if (param.Count(c => c == '-') != 1)
            {
                Console.WriteLine("Format error");
                return;
            }

            var user = new UserMapper().ToModel(param);

            if (!_userManager.CheckPassword(user))
            {
                Console.WriteLine("Login or password entered incorrectly");
                return;
            }

            try
            {
                _signManager.SignIn(user.Login);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Register(string param)
        {
            //Regex regex = new Regex(@"([a-zA-Z]+)\-([a-zA-z0-9_]+)\-([a-z]+@[a-z]+\.[a-z]+)\-([0-9]+)\-([0-9]+)"); //login - password - email - phone - age

            if (param.Count(c => c == '-') != 4)
            {
                Console.WriteLine("Format error");
                return;
            }

            var user = new UserMapper().ToModel(param);
            var commandResult = _userManager.RegisterUser(user);
            if (!commandResult.Success)
            {
                Console.WriteLine(commandResult.ErrorMessage);
                return;
            }

            _signManager.SignIn(user.Login);
        }


        public void GetMyProfile(string param)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            var user = _userManager.GetUser(_signManager.GetUserLogin());
            Console.WriteLine("Login: " + user.Login);
            Console.WriteLine("Email: " + user.Email);
            Console.WriteLine("Phone: " + user.Phone);
            Console.WriteLine("Age: " + user.Age);
            Console.WriteLine("--------------------");
            Console.WriteLine("Skills: ");
            GetMySkills("");
        }

        public void GetMySkills(string param)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            var result = _skillManager.GetSkillLevelDict(_signManager.GetUserLogin());
            if (!result.Success)
            {
                Console.WriteLine(result.ErrorMessage);
                return;
            }

            foreach (var keyValuePair in result.ModelResult)
            {
                Console.WriteLine(keyValuePair.Key + " - " + keyValuePair.Value);
            }
        }
    }
}
