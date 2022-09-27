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
    public class MaterialStateRepository : IRepository<MaterialState>
    {
        private readonly ApplicationContext _dbContext;

        public MaterialStateRepository(ApplicationContext applicationContext)
        {
            _dbContext = applicationContext;
        }

        public async Task AddAsync(MaterialState entity)
        {
            _dbContext.MaterialStates.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<MaterialState> GetAsync(int id)
        {
            var materialState = await _dbContext.MaterialStates.Include(c => c.Material).Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (materialState is null)
            {
                throw new ArgumentException("not found material in database");
            }

            return materialState;
        }

        public IQueryable<MaterialState> GetAll()
        {
            return _dbContext.MaterialStates.Include(c => c.Material).Include(c => c.User);
        }

        public async Task RemoveAsync(int id)
        {
            var toRemove = await _dbContext.MaterialStates.FirstOrDefaultAsync(c => c.Id == id);
            if (toRemove is null)
            {
                throw new ArgumentException("There is no MaterialStates with this id");
            }

            _dbContext.MaterialStates.Remove(toRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(MaterialState entity)
        {
            var toUpdate = await _dbContext.MaterialStates.FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (toUpdate is null)
            {
                throw new ArgumentException("There is no materialState with id that equals id of materialState param");
            }

            _dbContext.MaterialStates.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
