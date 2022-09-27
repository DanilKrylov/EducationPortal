using EducationPortal.Application.Interfaces;
using EducationPortal.Web.Filters;
using EducationPortal.Web.Mappers;
using EducationPortal.Web.Options;
using EducationPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        public HomeController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [PageNumberFilter]
        [SearchStringFilter]
        public async Task<IActionResult> Index(string searchString, int pageNumber)
        {
            var pageLength = HttpContext.RequestServices.GetService<IOptions<PageOptions>>().Value.PageLength;
            var pageView = new CoursePageViewModel()
            {
                PageNumber = pageNumber,
                CourseList = CourseMapper.ToViewModels((await _courseService.GetPublishedCoursesForPageAsync(pageNumber, pageLength, searchString)).ModelResult),
                PageCount = await _courseService.GetPageCountAsync("", pageLength),
                SearchString = searchString,
            };

            return View(pageView);
        }

        [HttpPost]
        [SearchStringFilter]
        public async Task<IActionResult> Index(string searchString)
        {
            if(searchString is null)
            {
                searchString = "";
            }

            var pageLength = HttpContext.RequestServices.GetService<IOptions<PageOptions>>().Value.PageLength;
            return View(new CoursePageViewModel()
            {
                PageNumber = 1,
                CourseList = CourseMapper.ToViewModels((await _courseService.GetPublishedCoursesForPageAsync(1, pageLength, searchString)).ModelResult),
                PageCount = await _courseService.GetPageCountAsync(searchString, pageLength),
                SearchString = searchString
            });
        }
    }
}
