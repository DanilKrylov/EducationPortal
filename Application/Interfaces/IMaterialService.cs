using EducationPortal.Domain.Models;
using EducationPortal.Domain.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Interfaces
{
    public interface IMaterialService
    {
        Task<CommandResult> AddMaterialToCourseAsync(Material material, string userName, int courseId);

        Task<CommandResult> RemoveMaterialFromCourseAsync(int materialId, string userName, int courseId);

        Task<ModelCommandResult<Material>> GetMaterialAsync(int id);

        Task<ModelCommandResult<List<Material>>> GetUnaddedMaterialsForPageAsync(int pageNumber, int pageLength, int courseId, string userName, string searchString);

        Task<ModelCommandResult<int>> GetPageCountAsync(int pageNumber, int pageLength, int courseId, string userName, string searchString);

        Task<CommandResult> AddExistingMaterialAsync(int materialId, int courseId, string userName);

        Task<CommandResult> CompleteMaterialStateAsync(int materialStateId, string userName);
    }
}
