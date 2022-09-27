using EducationPortal.Application.Extensions;
using EducationPortal.Application.Interfaces;
using EducationPortal.Application.Validators;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<CourseState> _courseStateRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<MaterialState> _materialStateRepository;

        public CourseService(IRepository<Course> courseRepository, IRepository<CourseState> courseStateRepository, UserManager<User> userManager, IRepository<MaterialState> materialStateRepository)
        {
            _courseRepository = courseRepository;
            _courseStateRepository = courseStateRepository;
            _userManager = userManager;
            _materialStateRepository = materialStateRepository;
        }

        public async Task<CommandResult<Dictionary<string, string>>> AddCourseAsync(Course course)
        {
            var validationResult = await new CourseInitialValidator(this).ValidateAsync(course);
            if (!validationResult.IsValid)
            {
                return new CommandResult<Dictionary<string, string>>()
                {
                    ErrorMessage = validationResult.Errors.ToDictionary(),
                    Success = false,
                };
            }

            await _courseRepository.AddAsync(course);
            return new CommandResult<Dictionary<string, string>>() { Success = true };
        }

        public async Task<ModelCommandResult<Course, string>> GetCourseAsync(int id)
        {
            try
            {
                return new ModelCommandResult<Course, string>()
                {
                    Success = true,
                    ModelResult = await _courseRepository.GetAsync(id)
                };
            }
            catch(Exception ex)
            {
                return new ModelCommandResult<Course, string>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ModelCommandResult<List<Course>>> GetPublishedCoursesForPageAsync(int pageNumber, int pageLength, string searchString)
        {
            if(pageNumber < 1)
            {
                pageNumber = 1;
            }

            if(pageLength < 1)
            {
                pageLength = 1;
            }

            var courses = await _courseRepository.GetAll().Where(c => c.Published && (c.SkillList.Any(k => k.Name.ToLower().Contains(searchString.ToLower()) ||
                               c.Name.ToLower().Contains(searchString.ToLower()))))
                   .Skip((pageNumber - 1) * pageLength).Take(pageLength).ToListAsync();

            return new ModelCommandResult<List<Course>>() { Success = true, ModelResult = courses };
        }

        public async Task<ModelCommandResult<List<Course>>> GetCoursesForCreatorAsync(string userLogin)
        {
            return new ModelCommandResult<List<Course>>()
            {
                ModelResult = await _courseRepository.GetAll().Where(c => c.UserCreatorLogin == userLogin).ToListAsync(),
                Success = true,
            };
        }

        public async Task<ModelCommandResult<List<CourseState>>> GetStartedCoursesAsync(string userName)
        {
            var courses = await _courseStateRepository.GetAll().Where(c => c.User.UserName == userName).ToListAsync();
            return new ModelCommandResult<List<CourseState>>()
            {
                Success = true,
                ModelResult = courses,
            };
        }

        public async Task<bool> NameIsUniqueAsync(Course course)
        {
            return await _courseRepository.GetAll().Where(c => c.Id != course.Id).FirstOrDefaultAsync(c => c.Name == course.Name) is null;
        }

        public async Task<CommandResult> PublishCourseAsync(int courseId, string userName)
        {
            Course course;
            try
            {
                course = await _courseRepository.GetAsync(courseId);
                if (course.UserCreatorLogin != userName)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return new CommandResult()
                {
                    Success = false,
                    ErrorMessage = "Course does not exist or user is not owner of course",
                };
            }
            var validationResult = await new CourseFullValidator(this).ValidateAsync(course);
            if (!validationResult.IsValid)
            {
                return new CommandResult()
                {
                    Success = false,
                    ErrorMessage = "Course is not valid",
                };
            }

            course.Published = true;
            await _courseRepository.UpdateAsync(course);
            return new CommandResult() { Success = true };
        }

        public async Task<CommandResult> RemoveCourseAsync(int courseId, string userName)
        {
            Course course;
            try
            {
                course = await _courseRepository.GetAsync(courseId);
                if (course.UserCreatorLogin != userName)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return new CommandResult()
                {
                    Success = false,
                    ErrorMessage = "Course does not exist or user is not owner of course",
                };
            }

            await _courseRepository.RemoveAsync(courseId);
            return new CommandResult() { Success = true };
        }

        public async Task<ModelCommandResult<CourseState>> StartOrGetCourseAsync(int courseId, string userName)
        {
            var courseStateInDb = await _courseStateRepository.GetAll().FirstOrDefaultAsync(c => c.Course.Id == courseId && c.User.UserName == userName);
            if (courseStateInDb is not null)
            {
                return new ModelCommandResult<CourseState>()
                {
                    Success = true,
                    ModelResult = courseStateInDb,
                };
            }

            var courseCommandRessult = await GetCourseAsync(courseId);

            if (!courseCommandRessult.Success)
            {
                return new ModelCommandResult<CourseState>()
                {
                    Success = false,
                    ErrorMessage = courseCommandRessult.ErrorMessage,
                };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            var courseState = new CourseState()
            {
                Course = courseCommandRessult.ModelResult,
                User = user
            };

            var userMaterialStates = _materialStateRepository.GetAll().Where(c => c.User.UserName == userName);
            foreach (var material in courseCommandRessult.ModelResult.Materials)
            {
                var createdState = userMaterialStates.FirstOrDefault(c => c.Material.Id == material.Id);
                if (createdState is not null)
                {
                    courseState.MaterialStates.Add(createdState);
                    continue;
                }

                courseState.MaterialStates.Add(new MaterialState() { Material = material, User = user});
            }

            if (courseState.MaterialStates.All(c => c.Completed))
            {
                courseState.Completed = true;
            }

            await _courseStateRepository.AddAsync(courseState);
            return new ModelCommandResult<CourseState>() { Success= true, ModelResult= courseState };
        }

        public async Task CheckCompletingOfCoursesIncluding(int materialStateId, string userName)
        {
            var courseStates = await _courseStateRepository.GetAll()
               .Where(c => c.User.UserName == userName && !c.Completed && c.MaterialStates.All(m => m.Completed) &&
                           c.MaterialStates.FirstOrDefault(k => k.Id == materialStateId) != null).ToListAsync();

            foreach (var courseState in courseStates)
            {
                courseState.Completed = true;
                await _courseStateRepository.UpdateAsync(courseState);
            }
        }

        public async Task<int> GetPageCountAsync(string searchString, int pageLength)
        {
            return (int)Math.Ceiling(await _courseRepository.GetAll().Where(c => c.Published && (c.SkillList.Any(k => k.Name.ToLower().Contains(searchString.ToLower()) ||
                               c.Name.ToLower().Contains(searchString.ToLower()))))
                   .CountAsync() / (double) pageLength);
        }



        /*

        public List<Course> GetCoursesForCreator(string userLogin)
        {
            return GetCourses().Where(course => course.UserCreatorLogin == userLogin).ToList();
        }

        public List<Course> GetCourses()
        {
            return _courseRepository.GetAll().ToList();
        }

        public CommandResult<List<Course>> GetCoursesForPage(int pageNumber, int pageLength)
        {
            if (pageNumber <= 0)
            {
                return new CommandResult<List<Course>>()
                {
                    Success = false,
                    ErrorMessage = "page number must be greater than zero",
                };
            }

            var courses = GetCourses().Skip((pageNumber - 1) * pageLength).Take(pageLength).ToList();

            if (courses.Count == 0)
            {
                return new CommandResult<List<Course>>()
                {
                    Success = false,
                    ErrorMessage = "there are no courses for this page",
                };
            }

            return new CommandResult<List<Course>>()
            {
                Success = true,
                ModelResult = courses,
            };
        }

        public void CheckCompletingOfCoursesIncluding(int courseId)
        {
            var courseStates = _courseStateRepository.GetAll()
                                .Where(c => c.Course.Id == courseId && !c.Completed && c.MaterialStates.All(k => k.Completed));

            foreach (var courseState in courseStates)
            {
                courseState.Completed = true;
                _courseStateRepository.Update(courseState);
            }
        }

        public CommandResult<CourseState> GetStartedCourse(int courseId, string userName)
        {
            var courseState = _courseStateRepository.GetAll().FirstOrDefault(c => c.Course.Id == courseId && c.User.Login == userName);
            if (courseState is null)
            {
                return new CommandResult<CourseState>()
                {
                    Success = false,
                    ErrorMessage = "There are no course with this id that you started"
                };
            }

            return new CommandResult<CourseState>()
            {
                Success = true,
                ModelResult = courseState,
            };
        }

        public CommandResult<List<CourseState>> GetStartedCourses(string userName)
        {
            var courseStates = _courseStateRepository.GetAll().Where(c => c.User.Login == userName).ToList();

            if (courseStates.Count == 0)
            {
                return new CommandResult<List<CourseState>>() { Success = false, ErrorMessage = "There are no started course for you" };
            }

            return new CommandResult<List<CourseState>>() { Success = true, ModelResult = courseStates };
        }

        private List<MaterialState> GetMaterialStates(string userName)
        {
            return _materialStateRepository.GetAll().Where(c => c.User.Login == userName).ToList();
        }*/
    }
}
