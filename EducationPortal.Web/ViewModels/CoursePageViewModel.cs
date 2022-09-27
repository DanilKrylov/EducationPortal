using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class CoursePageViewModel
    {
        public string SearchString { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public List<CourseViewModel> CourseList { get; set; }
    }
}
