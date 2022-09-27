using EducationPortal.Web.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace EducationPortal.Web.Filters
{
    public class SearchStringFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var searchString = (string)context.ActionArguments["searchString"];
                if(searchString is null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                context.ActionArguments["searchString"] = "";
            }
        }
    }
}
