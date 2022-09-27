using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models.Materials
{
    public class Video : Material
    {
        public Video(string name, MaterialData data)
        {
            Name = name;
            Data = data;
        }

        public Video(string name)
        {
            Name = name;
        }

        public MaterialData Data { get; set; }
    }
}
