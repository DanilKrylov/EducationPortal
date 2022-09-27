using EducationPortal.Domain.Models;
using EducationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.Mappers
{
    public static class SkillMapper
    {
        public static Skill ToModel(AddSkillViewModel viewModel)
        {
            return new Skill(viewModel.Name);
        }
    }
}
