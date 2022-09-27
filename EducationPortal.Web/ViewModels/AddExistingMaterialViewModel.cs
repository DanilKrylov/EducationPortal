using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class AddExistingMaterialPageViewModel
    {
        public int CourseId { get; set; }

        public List<Material> Materials { get; set; }

        public string SearchString { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public AddExistingMaterialPageViewModel(int courseId, List<Material> materials, int pageNumber, int pageCount, string searchString)
        {
            CourseId = courseId;
            Materials = materials;
            PageNumber = pageNumber;
            PageCount = pageCount;
            SearchString = searchString;
        }
    }
}
