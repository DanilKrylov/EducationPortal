using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models
{
    public class Skill : Entity
    {
        public Skill(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}
