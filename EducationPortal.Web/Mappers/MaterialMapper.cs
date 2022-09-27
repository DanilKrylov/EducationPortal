using EducationPortal.Domain.Models;
using EducationPortal.Domain.Models.Materials;
using EducationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Web.Mappers
{
    public static class MaterialMapper
    {
        public static Link ToModel(AddLinkViewModel viewModel)
        {
            if (!viewModel.Source.StartsWith("https://"))
            {
                viewModel.Source = "https://" + viewModel.Source;
            }

            return new Link(viewModel.Source, viewModel.Name);
        }

        public static Pdf ToModel(AddPdfViewModel viewModel)
        {
            byte[] data = null;

            using (var binaryReader = new BinaryReader(viewModel.Data.OpenReadStream()))
            {
                data = binaryReader.ReadBytes((int)viewModel.Data.Length);
            }

            return new Pdf(viewModel.NumberOfPages, viewModel.Name, new MaterialData { ByteData = data });
        }

        public static Video ToModel(AddVideoViewModel viewModel)
        {
            byte[] data = null;

            using (var binaryReader = new BinaryReader(viewModel.Data.OpenReadStream()))
            {
                data = binaryReader.ReadBytes((int)viewModel.Data.Length);
            }

            return new Video(viewModel.Name, new MaterialData { ByteData = data});
        }
    }
}
