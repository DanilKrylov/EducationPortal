using EducationPortal.Data.Context;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Data.Repositories
{
    public class SkillRepository : IRepository<Skill>
    {
        private readonly ApplicationContext _dbContext;
        public SkillRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Skill entity)
        {
            _dbContext.Skills.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Skill> GetAsync(int id)
        {
            var skill = await _dbContext.Skills.FirstOrDefaultAsync(c => c.Id == id);
            if (skill is null)
            {
                throw new ArgumentException("not found material in database");
            }

            return skill;
        }

        public IQueryable<Skill> GetAll()
        {
            return _dbContext.Skills;
        }

        public async Task RemoveAsync(int id)
        {
            var toRemove = await _dbContext.Skills.FirstOrDefaultAsync(c => c.Id == id);
            if (toRemove is null)
            {
                throw new ArgumentException("There is no course with this id");
            }

            _dbContext.Skills.Remove(toRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Skill skill)
        {
            var skillToUpdate = await _dbContext.Skills.FirstOrDefaultAsync(c => c.Id == skill.Id);
            if (skillToUpdate is null)
            {
                throw new ArgumentException("There is no course with id that equals id of course param");
            }

            _dbContext.Skills.Update(skill);
            await _dbContext.SaveChangesAsync();
        }
    }
}
