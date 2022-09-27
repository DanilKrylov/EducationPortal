using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Models;
using EducationPortal.Web.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationPortal.Web.Controllers
{
    [Authorize]
    public class CoursePassageController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IMaterialService _materialService;

        public CoursePassageController(ICourseService courseService, IMaterialService materialService)
        {
            _courseService = courseService;
            _materialService = materialService;
        }

        public async Task<IActionResult> StartOrGetCourse(int courseId)
        {
            var commandResult =await _courseService.StartOrGetCourseAsync(courseId, User.Identity.Name);
            if (!commandResult.Success)
            {
                return StatusCode(404);
            }

            return View("../CoursePassage/CoursePassage", CourseStateMapper.ToViewModel(commandResult.ModelResult));
        }

        public async Task<IActionResult> CompleteMaterial(int materialStateId, int backCourseId)
        {
            var result = await _materialService.CompleteMaterialStateAsync(materialStateId, User.Identity.Name);
            if (result.Success)
            {
                await _courseService.CheckCompletingOfCoursesIncluding(materialStateId, User.Identity.Name);
                return RedirectToAction("StartOrGetCourse", "CoursePassage", new { courseId = backCourseId});
            }

            return StatusCode(400);
        }
    }
}
