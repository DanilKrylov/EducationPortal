using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models
{
    public class CourseState : Entity
    {
        public User User { get; set; }

        public Course Course { get; set; }

        public List<MaterialState> MaterialStates { get; set; } = new();

        public bool Completed { get; set; }
    }
}
