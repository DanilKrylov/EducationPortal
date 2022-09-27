using EducationPortal.Domain.Models;
using EducationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.Mappers
{
    public static class CourseStateMapper
    {
        public static CourseStateViewModel ToViewModel(CourseState courseState)
        {
            return new CourseStateViewModel()
            {
                Completed = courseState.Completed,
                Course = courseState.Course,
                MaterialStates = courseState.MaterialStates,
            };
        }
    }
}
