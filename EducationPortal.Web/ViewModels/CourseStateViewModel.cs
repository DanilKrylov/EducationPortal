using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class CourseStateViewModel
    {
        public Course Course { get; set; }

        public List<MaterialState> MaterialStates { get; set; } = new();

        public bool Completed { get; set; }
    }
}
