using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public Dictionary<string, int> SkillLevelDict { get; set; }

        public List<Course> CreatedCourse { get; set; }

        public List<CourseState> StartedCourse { get; set; }


        public ProfileViewModel(string userName, int age, string email, Dictionary<string, int> skillLevelDict, List<Course> createdCourse, List<CourseState> startedCourse)
        {
            UserName = userName;
            Age = age;
            Email = email;
            CreatedCourse = createdCourse;
            StartedCourse = startedCourse;
            SkillLevelDict = skillLevelDict;
        }
    }
}
