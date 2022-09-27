using EducationPortal.Application.Interfaces;
using EducationPortal.Application.Services;
using EducationPortal.Application.Validators;
using EducationPortal.ConsoleView.Helpers;
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
    public class CourseManagementController
    {
        private readonly ISignService _signManager;
        private readonly ICourseService _courseManager;
        private readonly IMaterialService _materialManager;
        private readonly ICourseBuilder _courseBuilder;
        private readonly ISkillService _skillManager;

        public CourseManagementController(ISignService signManager, ICourseBuilder courseBuilder, ISkillService skillManager, ICourseService courseManager, IMaterialService materialManager)
        {
            _signManager = signManager;
            _courseBuilder = courseBuilder;
            _skillManager = skillManager;
            _courseManager = courseManager;
            _materialManager = materialManager;
        }

        public void DeleteMyCreatedCourse(string courseId)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("You must authorize");
                return;
            }

            Course course;
            try
            {
                course = _courseManager.GetCourse(Convert.ToInt32(courseId));
                if (course.UserCreatorLogin != _signManager.GetUserLogin())
                {
                    throw new ArgumentException();
                }

                _courseManager.DeleteCourse(course.Id);
            }
            catch (FormatException)
            {
                Console.WriteLine("course_id must be integer");
                return;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("the course does not exist or does not belong to you");
                return;
            }
        }

        public void RemakeMyCreatedCourse(string courseId)
        {
            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("You must authorize");
                return;
            }

            var result = _courseBuilder.StartCourseChanging(Convert.ToInt32(courseId));
            if (!result.Success)
            {
                Console.WriteLine(result.ErrorMessage);
                return;
            }

            StartCourseBuilding();
        }

        public void MyCreatedCourses(string param)
        {
            if (param.Trim() != "view")
            {
                Console.WriteLine("error_command");
            }

            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("You must authorize");
            }

            var userCourses = _courseManager.GetCoursesForCreator(_signManager.GetUserLogin());
            if (userCourses == null || userCourses.Count == 0)
            {
                Console.WriteLine("You dont have any created courses");
                return;
            }

            foreach (var course in userCourses)
            {
                Console.WriteLine(course.GetInfo());
            }
        }

        public void AddCourse(string param)
        {
            //var regex = new Regex(@"([a-zA-Z0-9]+)\-([a-zA-Z]+)"); // name-description

            if (!_signManager.IsSignIn())
            {
                Console.WriteLine("you must authorize");
                return;
            }

            try
            {
                var course = new CourseMapper().ToModel(param);
                var resultOfCreating = _courseBuilder.StartCourseCreating(course);

                if (!resultOfCreating.Success)
                {
                    Console.WriteLine(resultOfCreating.ErrorMessage);
                    return;
                }

                StartCourseBuilding();
            }
            catch
            {
                Console.WriteLine("Format error");
            }
        }

        private void StartCourseBuilding()
        {
            var courseAddCommandBuilder = new CommandBuilder();
            BuildCommandsForCourseChanging(courseAddCommandBuilder, new CourseBuildHelper(_materialManager, _courseBuilder));

            while (true)
            {
                WriteCommandsForCourseBuilding();
                var userCommand = Console.ReadLine().Trim();

                if (userCommand == "end")
                {
                    var resultOfSave = _courseBuilder.SaveCourse();

                    if (!resultOfSave.Success)
                    {
                        Console.WriteLine(resultOfSave.ErrorMessage);
                        continue;
                    }

                    Console.WriteLine("succes");
                    return;
                }

                try
                {
                    var command = userCommand[..userCommand.IndexOf(" ")];
                    var commandParam = userCommand.Substring(userCommand.IndexOf(" ") + 1, userCommand.Length - userCommand.IndexOf(" ") - 1);
                    courseAddCommandBuilder.Invoke(command, commandParam);

                    Console.WriteLine("succes");
                }
                catch
                {
                    Console.WriteLine("error command");
                }
            }
        }

        private void WriteCommandsForCourseBuilding()
        {
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("end (end creating course)");
            Console.WriteLine("add_link <link>");
            Console.WriteLine("add_pdf <number of pages>-<author>");
            Console.WriteLine("add_video <video length>-<quality>");
            Console.WriteLine("add_skill <skill_name>");
            Console.WriteLine("add_existing_material <id_of_material>");
            Console.WriteLine("view_materials <page_number>");
            Console.WriteLine("view_materials in_course");
            Console.WriteLine("view_skills in_course");
            Console.WriteLine("remove_skill <skill_name>");
            Console.WriteLine("remove_material <material_id>");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine();
        }

        private void BuildCommandsForCourseChanging(CommandBuilder commandBuilder, CourseBuildHelper courseAddHelper)
        {
            commandBuilder.AddCommand("add_link", courseAddHelper.AddLink);
            commandBuilder.AddCommand("add_pdf", courseAddHelper.AddPdf);
            commandBuilder.AddCommand("add_video", courseAddHelper.AddVideo);
            commandBuilder.AddCommand("add_skill", courseAddHelper.AddSkill);
            commandBuilder.AddCommand("add_existing_material", courseAddHelper.AddExistingMaterial);
            commandBuilder.AddCommand("view_materials", courseAddHelper.ViewMaterials);
            commandBuilder.AddCommand("view_skills", courseAddHelper.ViewSkills);
            commandBuilder.AddCommand("remove_skill", courseAddHelper.RemoveSkill);
            commandBuilder.AddCommand("remove_material", courseAddHelper.RemoveMaterial);
            commandBuilder.Build();
        }
    }
}
