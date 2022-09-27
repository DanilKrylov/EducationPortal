using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EducationPortal.Domain.Models
{
    public class Course : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string UserCreatorLogin { get; set; }

        public bool Published { get; set; }

        public List<Skill> SkillList { get; set; } = new();

        public List<Material> Materials { get; set; } = new();

#warning to delete
        public void AddSkill(Skill skill)
        {
            if (SkillList.FirstOrDefault(s => s.Name == skill.Name) == null)
            {
                SkillList.Add(skill);
                return;
            }

            throw new ArgumentException("this skill has allready been added");
        }

        public void AddMaterial(Material material)
        {
            Materials.Add(material);
        }
    }
}
