using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models.Materials
{
    public class Pdf : Material
    {
        public Pdf(int numberOfPages, string name, MaterialData data)
        {
            NumberOfPages = numberOfPages;
            Name = name;
            Data = data;
        }

        public Pdf(int numberOfPages, string name)
        {
            NumberOfPages = numberOfPages;
            Name = name;
        }

        public MaterialData Data { get; set; }

        public int NumberOfPages { get; set; }
    }
}
