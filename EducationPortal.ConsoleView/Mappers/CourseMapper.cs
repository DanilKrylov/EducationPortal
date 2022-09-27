using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Mappers
{
    internal class CourseMapper : IMapper<Course>
    {
        public Course ToModel(string userInput)
        {
            var courseParams = userInput.Split('-');
            if(courseParams.Length == 2)
            {
                return new Course()
                {
                    Name = courseParams[0],
                    Description = courseParams[1],
                };
            }

            throw new ArgumentException("Format error");
        }
    }
}
