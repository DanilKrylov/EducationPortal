using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Mappers
{
    internal interface IMapper<TModel> where TModel : Entity
    {
        TModel ToModel(string userInput);
    }
}
