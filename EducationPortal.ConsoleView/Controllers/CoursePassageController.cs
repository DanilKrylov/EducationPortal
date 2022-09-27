using EducationPortal.Application.Interfaces;
using EducationPortal.Application.Managers;
using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Controllers
{
    public class CoursePassageController
    {
        private readonly ICourseService _courseManager;
        private readonly ISignService _signManager;
        private readonly IMaterialService _materialManager;

        public CoursePassageController(ICourseService courseManager, ISignService signManager, IMaterialService materialManager)
        {
            _courseManager = courseManager;
            _signManager = signManager;
            _materialManager = materialManager;
        }

        public void ViewCourses(string pageNumber)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            int page;
            try
            {
                page = Convert.ToInt32(pageNumber);
            }
            catch
            {
                Console.WriteLine("page number must be integer");
                return;
            }

            var commandResult = _courseManager.GetCoursesForPage(page, 3);
            if (!commandResult.Success)
            {
                Console.WriteLine(commandResult.ErrorMessage);
                return;
            }

            foreach (var course in commandResult.ModelResult)
            {
                Console.WriteLine(course.GetInfo());
            }
        }

        public void StartCourse(string courseId)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            int id;
            try
            {
                id = Convert.ToInt32(courseId);
            }
            catch
            {
                Console.WriteLine("id must be integer");
                return;
            }

            var commandResult = _courseManager.StartCourse(id, _signManager.GetUserLogin());
            if (!commandResult.Success)
            {
                Console.WriteLine(commandResult.ErrorMessage);
                return;
            }

            Console.WriteLine("Succes, you can find this course at 'Course/MyStartedCourses'");
        }

        public void MyStartedCourses(string param)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            var commandResult = _courseManager.GetStartedCourses(_signManager.GetUserLogin());

            if (!commandResult.Success)
            {
                Console.WriteLine(commandResult.ErrorMessage);
                return;
            }

            foreach (var courseState in commandResult.ModelResult)
            {
                var toWrite = "";
                if (courseState.Completed)
                {
                    toWrite += "(completed)";
                }

                Console.WriteLine(toWrite + courseState.Id + ". " + courseState.Course.Name + " - " + courseState.Course.Description);
            }
        }

        public void ViewStartedCourse(string courseId)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            int id;
            try
            {
                id = Convert.ToInt32(courseId);
            }
            catch
            {
                Console.WriteLine("course id must be integer");
                return;
            }

            var result = _courseManager.GetStartedCourse(id, _signManager.GetUserLogin());
            if (!result.Success)
            {
                Console.WriteLine(result.ErrorMessage);
                return;
            }

            Console.WriteLine(result.ModelResult.GetInfo());
        }

        public void CompleteMaterial(string materialId)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            int id;
            try
            {
                id = Convert.ToInt32(materialId);
            }
            catch
            {
                Console.WriteLine("material id must be integer");
                return;
            }

            var result = _materialManager.CompleteMaterialState(id, _signManager.GetUserLogin());
            if (!result.Success)
            {
                Console.WriteLine(result.ErrorMessage);
                return;
            }

            Console.WriteLine("complete");
            _courseManager.CheckCompletingOfCoursesIncluding(id, _signManager.GetUserLogin());
        }
    }
}
