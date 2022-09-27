using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Interfaces
{
    public interface ICourseService
    {
        Task<ModelCommandResult<List<Course>>> GetPublishedCoursesForPageAsync(int pageNumber, int pageCount, string searchString);

        Task<int> GetPageCountAsync(string searchString, int pageLength);

        Task<ModelCommandResult<Course, string>> GetCourseAsync(int id);

        Task<CommandResult<Dictionary<string, string>>> AddCourseAsync(Course course);

        Task<bool> NameIsUniqueAsync(Course course);

        Task<CommandResult> PublishCourseAsync(int courseId, string userName);

        Task<CommandResult> RemoveCourseAsync(int courseId, string userName);

        Task<ModelCommandResult<List<Course>>> GetCoursesForCreatorAsync(string userLogin);

        Task<ModelCommandResult<List<CourseState>>> GetStartedCoursesAsync(string userName);

        Task<ModelCommandResult<CourseState>> StartOrGetCourseAsync(int courseId, string userName);

        Task CheckCompletingOfCoursesIncluding(int materialStateId, string userName);
    }
}
