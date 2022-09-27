using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Models;
using EducationPortal.Domain.Models.Materials;
using EducationPortal.Web.Filters;
using EducationPortal.Web.Mappers;
using EducationPortal.Web.Options;
using EducationPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace EducationPortal.Web.Controllers
{
    [Authorize]
    public class CourseManageController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IMaterialService _materialService;
        private readonly ISkillService _skillService;
        private readonly IConfiguration _configuration;

        public CourseManageController(ICourseService courseService, IMaterialService materialService, ISkillService skillService, IConfiguration configuration)
        {
            _courseService = courseService;
            _materialService = materialService;
            _skillService = skillService;
            _configuration = configuration;
        }

        public async Task<IActionResult> ViewCreatedCourse(int courseId)
        {
            var commandResult = await _courseService.GetCourseAsync(courseId);
            if (commandResult.Success)
            {
                if(commandResult.ModelResult.UserCreatorLogin != User.Identity.Name)
                {
                    return StatusCode(401);
                }

                return View(CourseMapper.ToViewModel(commandResult.ModelResult));
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveCourse(int courseId)
        {
            var commandResult = await _courseService.RemoveCourseAsync(courseId, User.Identity.Name);
            if (!commandResult.Success)
            {
                return StatusCode(400);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseCreateViewModel viewModel)
        {
            var course = CourseMapper.ToModel(viewModel, User.Identity.Name);
            var createResult = await _courseService.AddCourseAsync(course);

            if (createResult.Success)
            {
                return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = course.Id });
            }

            foreach(var err in createResult.ErrorMessage)
            {
                ModelState.AddModelError(err.Key, err.Value);
            }

            return View();
        }

        [PageNumberFilter]
        [SearchStringFilter]
        public async Task<IActionResult> AddExistingMaterialView(int courseId, string searchString, int pageNumber)
        {
            if (searchString is null)
            {
                searchString = "";
            }

            var pageLength = HttpContext.RequestServices.GetService<IOptions<PageOptions>>().Value.PageLength;
            var materials = await _materialService.GetUnaddedMaterialsForPageAsync(pageNumber, pageLength, courseId, User.Identity.Name, searchString);
            var pageCount = await _materialService.GetPageCountAsync(pageNumber, pageLength, courseId, User.Identity.Name, searchString);

            if(!pageCount.Success || !materials.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            var pageView = new AddExistingMaterialPageViewModel(courseId, materials.ModelResult, pageNumber, pageCount.ModelResult, "");

            return View(pageView);
        }

        [HttpPost]
        [SearchStringFilter]
        public async Task<IActionResult> AddExistingMaterialView(int courseId, string searchString)
        {
            if(searchString is null)
            {
                searchString = "";
            }

            var pageLength = HttpContext.RequestServices.GetService<IOptions<PageOptions>>().Value.PageLength;
            var materials = await _materialService.GetUnaddedMaterialsForPageAsync(1, pageLength, courseId, User.Identity.Name, searchString);
            var pageCount = await _materialService.GetPageCountAsync(1, pageLength, courseId, User.Identity.Name, searchString);

            if (!pageCount.Success || !materials.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            var pageView = new AddExistingMaterialPageViewModel(courseId, materials.ModelResult, 1, pageCount.ModelResult, "");

            return View(pageView);
        }

        public async Task<IActionResult> AddExistingMaterial(int courseId, int materialId)
        {
            var commandResult = await _materialService.AddExistingMaterialAsync(materialId, courseId, User.Identity.Name);

            return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId });
        }


        public async Task<IActionResult> AddLink(int courseId)
        {
            return View(new AddLinkViewModel() { CourseId = courseId});
        }

        [HttpPost]
        public async Task<IActionResult> AddLink(AddLinkViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var commandResult = await _materialService.AddMaterialToCourseAsync(MaterialMapper.ToModel(viewModel), User.Identity.Name, viewModel.CourseId);
            if (commandResult.Success)
            {
                return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = viewModel.CourseId });
            }

            return RedirectToAction();
        }

        public async Task<IActionResult> AddPdf(int courseId)
        {
            return View(new AddPdfViewModel() { CourseId = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> AddPdf(AddPdfViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var commandResult = await _materialService.AddMaterialToCourseAsync(MaterialMapper.ToModel(viewModel), User.Identity.Name, viewModel.CourseId);
            if (commandResult.Success)
            {
                return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = viewModel.CourseId });
            }

            return RedirectToAction();
        }

        public async Task<IActionResult> AddVideo(int courseId)
        {
            return View(new AddVideoViewModel() { CourseId = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo(AddVideoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var commandResult = await _materialService.AddMaterialToCourseAsync(MaterialMapper.ToModel(viewModel), User.Identity.Name, viewModel.CourseId);
            if (commandResult.Success)
            {
                return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = viewModel.CourseId });
            }

            return RedirectToAction();
        }

        public async Task<IActionResult> AddSkill(int courseId)
        {
            return View(new AddSkillViewModel() { CourseId = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill(AddSkillViewModel viewModel)
        {
            var commandResult = await _skillService.AddSkillToCourseAsync(SkillMapper.ToModel(viewModel), viewModel.CourseId, User.Identity.Name);

            if (commandResult.Success)
            {
                return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = viewModel.CourseId });
            }

            ModelState.AddModelError("Name", commandResult.ErrorMessage);
            return View(viewModel);
        }
        
        public async Task<IActionResult> RemoveMaterial(int courseId, int materialId)
        {
            await _materialService.RemoveMaterialFromCourseAsync(materialId, User.Identity.Name, courseId);
            return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = courseId });
        }

        public async Task<IActionResult> RemoveSkill(int courseId, int skillId)
        {
            await _skillService.RemoveSkillFromCourse(skillId, courseId, User.Identity.Name);
            return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = courseId });
        }

        public async Task<IActionResult> Publish(int courseId)
        {
            await _courseService.PublishCourseAsync(courseId, User.Identity.Name);
            return RedirectToAction("ViewCreatedCourse", "CourseManage", new { courseId = courseId });
        }

        public async Task<IActionResult> ViewPdf(int pdfId)
        {
            var commandResult = await _materialService.GetMaterialAsync(pdfId);

            if (commandResult.Success && commandResult.ModelResult is Pdf pdf)
            {
                return File(pdf.Data.ByteData, "application/pdf");
            }

            return StatusCode(404);
        }

        public async Task<IActionResult> ViewVideo(int videoId)
        {
            var commandResult = await _materialService.GetMaterialAsync(videoId);

            if (commandResult.Success && commandResult.ModelResult is Video video)
            {
                return File(video.Data.ByteData, "video/mp4");
            }

            return StatusCode(404);
        }
    }
}
