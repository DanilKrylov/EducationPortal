using EducationPortal.Domain.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Mappers
{
    internal class VideoMapper : IMapper<Video>
    {
        public Video ToModel(string userInput)
        {
            var videoProps = userInput.Split('-');
            if (videoProps.Length != 2)
            {
                throw new ArgumentException("Format error");
            }

            try
            {
                return new Video(Convert.ToInt32(videoProps[0]), Convert.ToInt32(videoProps[1]));
            }
            catch
            {
                throw new ArgumentException("Please use integer numbers");
            }
        }
    }
}
