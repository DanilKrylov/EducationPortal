using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models
{
    public class MaterialState : Entity
    {
        public int CourseStateId { get; set; }

        public List<CourseState> CourseStates { get; set; }

        public Material Material { get; set; }

        public User User { get; set; }

        public bool Completed { get; set; }
    }
}
