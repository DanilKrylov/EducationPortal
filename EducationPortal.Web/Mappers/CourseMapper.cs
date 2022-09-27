using EducationPortal.Domain.Models;
using EducationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.Mappers
{
    public static class CourseMapper
    {
        public static Course ToModel(CourseCreateViewModel viewModel, string userName)
        {
            return new Course()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                UserCreatorLogin = userName,
            };
        }

        public static CourseViewModel ToViewModel(Course model)
        {
            return new CourseViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                UserCreatorLogin = model.UserCreatorLogin,
                Published = model.Published,
                Materials = model.Materials,
                SkillList = model.SkillList,
            };
        }

        public static List<CourseViewModel> ToViewModels(ICollection<Course> courses)
        {
            return courses.Select(c => ToViewModel(c)).ToList();
        }
    }
}
