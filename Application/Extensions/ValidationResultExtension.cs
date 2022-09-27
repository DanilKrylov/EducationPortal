using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application.Extensions
{
    internal static class ValidationResultExtension
    {
        public static Dictionary<string, string> ToDictionary(this List<ValidationFailure> errors)
        {
            var dict = new Dictionary<string, string>();
            foreach(var error in errors)
            {
                dict[error.PropertyName] = error.ErrorMessage;
            }

            return dict;
        }
    }
}
