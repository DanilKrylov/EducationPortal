using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models
{
    public abstract class Material : Entity
    {
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}
