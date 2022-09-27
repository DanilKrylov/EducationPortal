using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using EducationPortal.Domain.Models.Materials;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<MaterialState> _materialStateRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<CourseState> _courseStateRepositoty;

        public MaterialService(IRepository<Material> materialRepository, IRepository<MaterialState> materialStateRepository, IRepository<Course> courseRepository, IRepository<CourseState> courseStateRepository)
        {
            _materialRepository = materialRepository;
            _materialStateRepository = materialStateRepository;
            _courseRepository = courseRepository;
            _courseStateRepositoty = courseStateRepository;
        }

        public async Task<CommandResult> AddMaterialToCourseAsync(Material material, string userName, int courseId)
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

            course.AddMaterial(material);
            await _courseRepository.UpdateAsync(course);

            return new CommandResult() { Success = true };
        }

        public async Task<CommandResult> RemoveMaterialFromCourseAsync(int materialId, string userName, int courseId)
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

            var toRemove = course.Materials.FirstOrDefault(c => c.Id == materialId); 
            if(toRemove is null)
            {
                return new CommandResult()
                {
                    Success = false,
                    ErrorMessage = "Material does not exist",
                };
            }

            course.Materials.Remove(toRemove);
            await _courseRepository.UpdateAsync(course);
            return new CommandResult(){ Success = true};
        }

        public async Task<ModelCommandResult<Material>> GetMaterialAsync(int id)
        {
            try
            {
                var material = await _materialRepository.GetAsync(id);
                return new ModelCommandResult<Material>()
                {
                    Success = true,
                    ModelResult = material,
                };
            }
            catch
            {
                return new ModelCommandResult<Material>()
                {
                    Success = false,
                    ErrorMessage = "material does not exist",
                };
            }
        }

        public async Task<ModelCommandResult<List<Material>>> GetUnaddedMaterialsForPageAsync(int pageNumber, int pageLength, int courseId, string userName, string searchString)
        {
            var course = await _courseRepository.GetAll().FirstOrDefaultAsync(c => c.Id == courseId && c.UserCreatorLogin == userName);

            if(course == null)
            {
                return new ModelCommandResult<List<Material>>()
                {
                    Success = false,
                    ErrorMessage = "there is no course state with this id"
                };
            }

            var materials = await _materialRepository.GetAll()
                        .Where(c => course.Materials.Select(k => k.Id).Contains(c.Id) == false && c.Name.ToLower().Contains(searchString.ToLower()))
                        .Skip((pageNumber - 1) * pageLength)
                        .Take(pageLength)
                        .ToListAsync();

            return new ModelCommandResult<List<Material>>()
            {
                Success = true,
                ModelResult = materials,
            };
        }

        public async Task<ModelCommandResult<int>> GetPageCountAsync(int pageNumber, int pageLength, int courseId, string userName, string searchString)
        {
            var course = await _courseRepository.GetAll().FirstOrDefaultAsync(c => c.Id == courseId && c.UserCreatorLogin == userName);

            if (course == null)
            {
                return new ModelCommandResult<int>()
                {
                    Success = false,
                    ErrorMessage = "there is no course state with this id"
                };
            }

            var countOfPages = (int)Math.Ceiling(await _materialRepository.GetAll()
                        .Where(c => course.Materials.Select(k => k.Id).Contains(c.Id) == false && c.Name.ToLower().Contains(searchString.ToLower()))
                        .CountAsync() / (double)pageLength);

            return new ModelCommandResult<int>()
            {
                Success = true,
                ModelResult = countOfPages
            };
        }

        public async Task<CommandResult> AddExistingMaterialAsync(int materialId, int courseId, string userName)
        {
            try
            {
                var course = await _courseRepository.GetAsync(courseId);
                if(course.UserCreatorLogin == userName)
                {
                    if(course.Materials.FirstOrDefault(c => c.Id == materialId) is not null)
                    {
                        return new CommandResult()
                        {
                            ErrorMessage = "this material is alredy in course",
                            Success = false,
                        };
                    }

                    course.Materials.Add(await _materialRepository.GetAsync(materialId));
                    await _courseRepository.UpdateAsync(course);

                    return new CommandResult() { Success = true };
                }

                throw new Exception();
            }
            catch
            {
                return new CommandResult() { Success= false , ErrorMessage="course does not exist or name of owner does not equal to user name"};
            }
        }
        public async Task<CommandResult> CompleteMaterialStateAsync(int materialStateId, string userName)
        {
            MaterialState materialState;
            try
            {
                materialState = await _materialStateRepository.GetAsync(materialStateId);
                if(materialState.User.UserName != userName)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return new CommandResult()
                {
                    ErrorMessage = "Material state does not exist",
                    Success = false,
                };
            }

            if (materialState.Completed)
            {
                return new CommandResult()
                {
                    Success = false,
                    ErrorMessage = "this material has been already completed by you"
                };
            }

            materialState.Completed = true;
            await _materialStateRepository.UpdateAsync(materialState);

            return new CommandResult()
            {
                Success = true,
            };
        }
    }
}
