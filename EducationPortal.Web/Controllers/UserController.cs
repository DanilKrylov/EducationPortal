using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Models;
using EducationPortal.Web.Filters;
using EducationPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICourseService _courseService;
        private readonly ISkillService _skillService;
        public UserController(UserManager<User> userManager, ICourseService courseService, ISkillService skillService)
        {
            _userManager = userManager;
            _courseService = courseService;
            _skillService = skillService;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.Users.FirstAsync(c => c.UserName == User.Identity.Name);
            var createdCourses = (await _courseService.GetCoursesForCreatorAsync(user.UserName)).ModelResult;
            var startedCourses = (await _courseService.GetStartedCoursesAsync(user.UserName)).ModelResult;
            var skills = (await _skillService.GetSkillLevelDictAsync(user.UserName)).ModelResult;


            var viewModel = new ProfileViewModel(user.UserName, user.Age, user.Email, skills, createdCourses, startedCourses);
            return View(viewModel);
        }
    }
}
