using EducationPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Application
{
    public class ModelCommandResult<ModelResultType, ErrorType>
    {
        public bool Success { get; set; }

        public ErrorType ErrorMessage { get; set; }

        public ModelResultType ModelResult { get; set; }
    }

    public class ModelCommandResult<ModelResultType>
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public ModelResultType ModelResult { get; set; }
    }

    public class CommandResult<ErrorType>
    {
        public bool Success { get; set; }

        public ErrorType ErrorMessage { get; set; }
    }

    public class CommandResult
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
