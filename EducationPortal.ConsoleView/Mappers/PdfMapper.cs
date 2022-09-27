using EducationPortal.Domain.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Mappers
{
    internal class PdfMapper : IMapper<Pdf>
    {
        public Pdf ToModel(string userInput)
        {
            var pdfProps = userInput.Split('-');
            if(pdfProps.Length != 2)
            {
                throw new ArgumentException("Format Error");
            }

            try
            {
                return new Pdf(Convert.ToInt32(pdfProps[0]), pdfProps[1]);
            }
            catch
            {
                throw new ArgumentException("Please use integer numbers for count of pages");
            }
        }
    }
}
