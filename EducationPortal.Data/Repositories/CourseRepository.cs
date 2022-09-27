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
    public class CourseRepository : IRepository<Course>
    {
        private readonly ApplicationContext _dbContext;
        public CourseRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Course entity)
        {
            _dbContext.Courses.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Course> GetAsync(int id)
        {
            var course = await _dbContext.Courses.Include(c => c.Materials).Include(c => c.SkillList).FirstOrDefaultAsync(c => c.Id == id);
            if (course is null)
            {
                throw new ArgumentException("not found course in database");
            }

            return course;
        }

        public IQueryable<Course> GetAll()
        {
            return _dbContext.Courses.Include(c => c.Materials).Include(c => c.SkillList);
        }

        public async Task RemoveAsync(int id)
        {
            var toRemove = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if(toRemove is null)
            {
                throw new ArgumentException("There is no course with this id");
            }

            _dbContext.Courses.Remove(toRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            var courseToUpdate = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);
            if(courseToUpdate is null)
            {
                throw new ArgumentException("There is no course with id that equals id of course param");
            }

            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
