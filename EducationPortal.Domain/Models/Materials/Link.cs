using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models.Materials
{
    public class Link : Material
    {
        public Link(string source, string name)
        {
            Source = source;
            Name = name;
        }

        public string Source { get; set; }
    }
}
