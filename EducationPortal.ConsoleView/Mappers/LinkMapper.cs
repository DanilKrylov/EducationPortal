using EducationPortal.Domain.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Mappers
{
    internal class LinkMapper : IMapper<Link>
    {
        public Link ToModel(string userInput)
        {
            return new Link(userInput);
        }
    }
}
