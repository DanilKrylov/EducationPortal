using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class AddVideoViewModel
    {
        public int CourseId { get; set; }

        public string Name { get; set; }

        public IFormFile Data { get; set; }
    }
}
