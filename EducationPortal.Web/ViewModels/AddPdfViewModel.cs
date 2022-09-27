using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class AddPdfViewModel
    {
        public int CourseId { get; set; }

        public IFormFile Data { get; set; }

        public int NumberOfPages { get; set; }

        public string Name { get; set; }
    }
}
