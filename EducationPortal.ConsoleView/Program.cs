using EducationPortal.Application.Interfaces;
using EducationPortal.Application.Managers;
using EducationPortal.Application.Services;
using EducationPortal.ConsoleView.Controllers;
using EducationPortal.ConsoleView.Helpers;
using EducationPortal.Data.Context;
using EducationPortal.Domain.Interfaces;
using EducationPortal.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EducationPortal.ConsoleView
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = Configure();

            var signManager = services.GetService<ISignService>();
            signManager.SignOut();
            AllCommands();

            while (true)
            {
                //repositories
                var userManager = services.GetService<IUserService>();
                var materialManager = services.GetService<IMaterialService>();
                var courseManager = services.GetService<ICourseService>();
                var skillManager = services.GetService<ISkillService>();
                var courseBuilder = services.GetService<ICourseBuilder>();
                //comandbuilders
                var mainCommandBuilder = new CommandBuilder();
                //controllers
                var authorizationController = new UserController(userManager, signManager, skillManager);
                var courseManController = new CourseManagementController(signManager, courseBuilder, skillManager, courseManager, materialManager);
                var coursePasController = new CoursePassageController(courseManager, signManager, materialManager);
                //start configure
                BuildMainCommands(mainCommandBuilder, authorizationController, courseManController, coursePasController);

                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine();

                var userMess = Console.ReadLine().Trim();

                if (userMess == "Authorization/LogOut")
                {
                    authorizationController.LogOut();
                    continue;
                }

                if (!userMess.Contains(" "))
                {
                    Console.WriteLine("Error command");
                    continue;
                }

                var command = userMess[..userMess.IndexOf(" ")];
                var param = userMess.Substring(userMess.IndexOf(" ") + 1, userMess.Length - userMess.IndexOf(" ") - 1);

                try
                {
                    mainCommandBuilder.Invoke(command, param);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void BuildMainCommands(CommandBuilder commandBuilder, UserController userController, CourseManagementController courseManController, CoursePassageController coursePassageController)
        {
            commandBuilder.AddCommand("Authorization/Register", userController.Register);
            commandBuilder.AddCommand("Authorization/Login", userController.Login);
            commandBuilder.AddCommand("User/GetMyProfile", userController.GetMyProfile);

            commandBuilder.AddCommand("Course/AddCourse", courseManController.AddCourse);
            commandBuilder.AddCommand("Course/MyCreatedCourses", courseManController.MyCreatedCourses);
            commandBuilder.AddCommand("Course/RemakeMyCreatedCourse", courseManController.RemakeMyCreatedCourse);
            commandBuilder.AddCommand("Course/DeleteMyCreatedCourse", courseManController.DeleteMyCreatedCourse);

            commandBuilder.AddCommand("Course/ViewStartedCourse", coursePassageController.ViewStartedCourse);
            commandBuilder.AddCommand("Course/MyStartedCourses", coursePassageController.MyStartedCourses);
            commandBuilder.AddCommand("Course/CompleteMaterial", coursePassageController.CompleteMaterial);
            commandBuilder.AddCommand("Course/ViewCourses", coursePassageController.ViewCourses);
            commandBuilder.AddCommand("Course/StartCourse", coursePassageController.StartCourse);

            commandBuilder.Build();
        }

        private static void AllCommands()
        {
            Console.WriteLine("Authorization/LogOut");
            Console.WriteLine("Authorization/Register <login>-<password>-<email>-<phone>-<age>");
            Console.WriteLine("Authorization/Login <login>-<password>");
            Console.WriteLine("User/GetMyProfile view");

            Console.WriteLine("Course/AddCourse <name>-<description>");
            Console.WriteLine("Course/MyCreatedCourses view");
            Console.WriteLine("Course/RemakeMyCreatedCourse <course_id>");
            Console.WriteLine("Course/DeleteMyCreatedCourse <course_id>");


            Console.WriteLine("Course/ViewStartedCourse <course_id>");
            Console.WriteLine("Course/MyStartedCourses view");
            Console.WriteLine("Course/CompleteMaterial <material_id>");
            Console.WriteLine("Course/ViewCourses <page_number>");
            Console.WriteLine("Course/StartCourse <course_id>");
        }

        private static IServiceProvider Configure()
        {
            var serviceCollection = new ServiceCollection();
            DependencyContainer.RegisterServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }
    }
}
