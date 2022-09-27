using EducationPortal.Application.Interfaces;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Validators
{
    public class CourseFullValidator : CourseInitialValidator
    {
        public CourseFullValidator(ICourseService courseManager) : base(courseManager)
        {
            RuleFor(course => course.Materials).Must(materials  => materials.Count > 0).WithMessage("You need to add at least one material to the course");
            RuleFor(course => course.SkillList).Must(skillList => skillList.Count > 0).WithMessage("You need to add at least one skill to the course");
        }
    }
}
