using EducationPortal.Application.Interfaces;
using EducationPortal.ConsoleView.Mappers;
using EducationPortal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView.Helpers
{
    internal class CourseBuildHelper
    {
        private readonly IMaterialService _materialRepository;
        private readonly ICourseBuilder _courseBuilder;

        public CourseBuildHelper(IMaterialService materialRepository, ICourseBuilder courseBuilder)
        {
            _materialRepository = materialRepository;
            _courseBuilder = courseBuilder;
        }

        public void AddLink(string linkText)
        {
            try
            {
                var link = new LinkMapper().ToModel(linkText);
                var result = _courseBuilder.AddLink(link);
                if (!result.Success)
                {
                    Console.WriteLine(result.ErrorMessage);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddPdf(string pdfInfo)
        {
            try
            {
                var pdf = new PdfMapper().ToModel(pdfInfo);
                var result = _courseBuilder.AddPdf(pdf);

                if (!result.Success)
                {
                    Console.WriteLine(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddVideo(string videoInfo)
        {
            try
            {
                var video = new VideoMapper().ToModel(videoInfo);
                var result = _courseBuilder.AddVideo(video);

                if (!result.Success)
                {
                    Console.WriteLine(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddSkill(string skillInfo)
        {
            try
            {
                var skill = new SkillMapper().ToModel(skillInfo);
                var result = _courseBuilder.AddSkill(skill);

                if (!result.Success)
                {
                    Console.WriteLine(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddExistingMaterial(string materialId)
        {
            try
            {
                var result = _courseBuilder.AddExistingMaterial(Convert.ToInt32(materialId));

                if (!result.Success)
                {
                    Console.WriteLine(result.ErrorMessage);
                }
            }
            catch(FormatException ex)
            {
                Console.WriteLine("Please use integer numbers for material id");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ViewMaterials(string param)
        {
            if(param == "in_course")
            {
                var materials = _courseBuilder.GetMaterials().ModelResult;

                if(materials.Count == 0)
                {
                    Console.WriteLine("no materials in this course");
                }

                int id = 0;
                foreach (var material in materials)
                {
                    Console.WriteLine(id++ +". " + material.GetInfo()[(material.GetInfo().IndexOf('.') + 1)..]);
                }

                return;
            }

            try
            {
                var page = Convert.ToInt32(param);
                var materials = _materialRepository.GetMaterialsForPage(10, page);

                foreach (var material in materials)
                {
                    Console.WriteLine(material.GetInfo());
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("error <page_number> format");
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ViewSkills(string param)
        {
            if(param == "in_course")
            {
                var skills = _courseBuilder.GetSkills().ModelResult;
                if(skills.Count == 0)
                {
                    Console.WriteLine("no skills in this course");
                }

                foreach (var skill in skills)
                {
                    Console.WriteLine(skill.Name);
                }
            }
        }

        public void RemoveSkill(string skillName)
        {
            try
            {
                _courseBuilder.RemoveSkill(skillName);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RemoveMaterial(string materialId)
        {
            int intId;
            try
            {
                intId = Convert.ToInt32(materialId);
            }
            catch (FormatException)
            {
                Console.WriteLine("material_id must be integer");
                return;
            }

            try
            {
                _courseBuilder.RemoveMaterial(intId);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
