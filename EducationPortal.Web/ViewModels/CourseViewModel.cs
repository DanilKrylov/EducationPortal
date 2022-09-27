using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string UserCreatorLogin { get; set; }

        public bool Published { get; set; }

        public List<Skill> SkillList { get; set; } = new();

        public List<Material> Materials { get; set; } = new();
    }
}
