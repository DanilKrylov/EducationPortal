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
    public class CourseStateRepository : IRepository<CourseState>
    {
        private readonly ApplicationContext _dbContext;

        public CourseStateRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CourseState entity)
        {
            _dbContext.CourseStates.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CourseState> GetAsync(int id)
        {
            var courseState = await _dbContext.CourseStates
                                        .Include(c => c.Course)
                                            .ThenInclude(c => c.SkillList)
                                        .Include(c => c.User)
                                        .Include(c => c.MaterialStates)
                                            .ThenInclude(c => c.Material)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (courseState is null)
            {
                throw new ArgumentException("not found courseState in database");
            }

            return courseState;
        }

        public IQueryable<CourseState> GetAll()
        {
            return _dbContext.CourseStates
                .Include(c => c.Course)
                    .ThenInclude(c => c.SkillList)
                .Include(c => c.User)
                .Include(c => c.MaterialStates)
                    .ThenInclude(c => c.Material);
        }

        public async Task RemoveAsync(int id)
        {
            var toRemove = await _dbContext.CourseStates.FirstOrDefaultAsync(c => c.Id == id);
            if (toRemove is null)
            {
                throw new ArgumentException("There is no courseState with this id");
            }

            _dbContext.CourseStates.Remove(toRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseState entity)
        {
            var toUpdate = await _dbContext.CourseStates.FirstOrDefaultAsync(course => course.Id == entity.Id);
            if (toUpdate is null)
            {
                throw new ArgumentException("There is no courseState with id that equals id of courseState param");
            }

            _dbContext.CourseStates.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
