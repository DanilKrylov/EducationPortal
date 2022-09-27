using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Mappers
{
    internal class SkillMapper : IMapper<Skill>
    {
        public Skill ToModel(string userInput)
        {
            return new Skill(userInput);
        }
    }
}
