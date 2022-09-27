using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<CourseState> _courseStateRepository;
        private readonly IRepository<Course> _courseRepository;

        public SkillService(IRepository<Skill> skillRepository, IRepository<CourseState> courseStateRepository, IRepository<Course> courseRepository)
        {
            _skillRepository = skillRepository;
            _courseStateRepository = courseStateRepository;
            _courseRepository = courseRepository;
        }

        public async Task<CommandResult> AddSkillToCourseAsync(Skill skill, int courseId, string userName)
        {
            try
            {
                var course = await _courseRepository.GetAsync(courseId);
                if(course.UserCreatorLogin != userName)
                {
                    throw new Exception();
                }

                if (course.Published)
                {
                    return new CommandResult() { Success = false, ErrorMessage = "Course is already published" };
                }

                if (course.SkillList.FirstOrDefault(c => c.Name == skill.Name) is not null)
                {
                    return new CommandResult() { ErrorMessage = "Skill with this name is alredy exist in your course", Success=false };
                }

                var skillInDb = await _skillRepository.GetAll().FirstOrDefaultAsync(c => c.Name == skill.Name);
                if (skillInDb is not null)
                {
                    course.AddSkill(skillInDb);
                }
                else
                {
                    course.AddSkill(skill);
                }

                await _courseRepository.UpdateAsync(course);
            }
            catch
            {
                return new CommandResult() { Success = false, ErrorMessage = "Course does not exist or user is not owner of course" };
            }

            return new CommandResult() { Success = true };
        }

        public async Task<CommandResult> RemoveSkillFromCourse(int skillId, int courseId, string userName)
        {
            Course course;
            try
            {
                course = await _courseRepository.GetAsync(courseId);
                if (course.UserCreatorLogin != userName)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return new CommandResult()
                {
                    Success = false,
                    ErrorMessage = "Course does not exist or user is not owner of course",
                };
            }

            if (course.Published)
            {
                return new CommandResult() { Success = false, ErrorMessage = "Course is already published" };
            }

            var toRemove = course.SkillList.FirstOrDefault(c => c.Id == skillId);
            if (toRemove is null)
            {
                return new CommandResult()
                {
                    Success = false,
                    ErrorMessage = "Skill does not exist",
                };
            }

            course.SkillList.Remove(toRemove);
            await _courseRepository.UpdateAsync(course);
            return new CommandResult() { Success = true };
        }

        /*public void AddSkill(Skill skill)
        {
            _skillRepository.Add(skill);
        }

        public Skill GetSkill(string skillName)
        {
            var skill = _skillRepository.GetAll().FirstOrDefault(s => s.Name == skillName);

            if (skill is null)
            {
                throw new ArgumentException("There is no skill with this name");
            }

            return skill;
        }*/

        public async Task<ModelCommandResult<Dictionary<string, int>>> GetSkillLevelDictAsync(string userName)
        {
            var courseStates = _courseStateRepository.GetAll().Where(c => c.User.UserName == userName && c.Completed);

            var result = new ModelCommandResult<Dictionary<string, int>>()
            {
                Success = true,
                ModelResult = new Dictionary<string, int>()
            };

            foreach (var state in courseStates)
            {
                foreach (var skill in state.Course.SkillList)
                {
                    if (result.ModelResult.ContainsKey(skill.Name))
                    {
                        result.ModelResult[skill.Name]++;
                    }
                    else
                    {
                        result.ModelResult[skill.Name] = 1;
                    }
                }
            }

            return result;
        }
    }
}
