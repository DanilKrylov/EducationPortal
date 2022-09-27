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
    public class CourseInitialValidator : AbstractValidator<Course>
    {
        public CourseInitialValidator(ICourseService courseService)
        {
            RuleFor(course => course.Name).Length(5, 50).WithMessage("course name length must be between 5 and 50");
            RuleFor(course => course).MustAsync((course, _) => courseService.NameIsUniqueAsync(course)).WithMessage("course with this name is already exist").OverridePropertyName("Name");
            RuleFor(course => course.Description).Length(10, 100).WithMessage("course description length must be between 10 and 100");
        }
    }
}
