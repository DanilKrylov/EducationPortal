using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Interfaces
{
    public interface ISkillService
    {
        Task<CommandResult> AddSkillToCourseAsync(Skill skill, int courseId, string userName);

        Task<CommandResult> RemoveSkillFromCourse(int skillId, int courseId, string userName);

        //Skill GetSkill(string skillName);*/

        Task<ModelCommandResult<Dictionary<string, int>>> GetSkillLevelDictAsync(string userName);
    }
}
