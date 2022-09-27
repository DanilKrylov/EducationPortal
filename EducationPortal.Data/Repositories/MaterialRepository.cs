using EducationPortal.Data.Context;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using EducationPortal.Domain.Models.Materials;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Data.Repositories
{
    public class MaterialRepository : IRepository<Material>
    {
        private readonly ApplicationContext _dbContext;
        public MaterialRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Material entity)
        {
            _dbContext.Materials.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Material> GetAsync(int id)
        {
            Material material = await _dbContext.Materials.FirstOrDefaultAsync(c => c.Id == id);


            if (material is null)
            {
                throw new ArgumentException("not found material in database");
            }


            if (material is Pdf pdf)
            {
                pdf = await _dbContext.Pdfs.Include(c => c.Data).FirstOrDefaultAsync(c => c.Id == id);
                return pdf;
            }

            if (material is Video video)
            {
                video = await _dbContext.Videos.Include(c => c.Data).FirstOrDefaultAsync(c => c.Id == id);
                return video;
            }

            return material;
        }

        public IQueryable<Material> GetAll()
        {
            return _dbContext.Materials;
        }

        public async Task RemoveAsync(int id)
        {
            var toRemove = await _dbContext.Materials.FirstOrDefaultAsync(c => c.Id == id);
            if (toRemove is null)
            {
                throw new ArgumentException("There is no course with this id");
            }

            _dbContext.Materials.Remove(toRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Material material)
        {
            var materialToUpdate = await _dbContext.Materials.FirstOrDefaultAsync(m => material.Id == m.Id);
            if (materialToUpdate is null)
            {
                throw new ArgumentException("There is no course with id that equals id of course param");
            }

            _dbContext.Materials.Update(material);
            await _dbContext.SaveChangesAsync();
        }
    }
}
