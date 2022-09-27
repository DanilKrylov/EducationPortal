using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace EducationPortal.Web.Filters
{
    public class PageNumberFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var pageNumber = (int)context.ActionArguments["pageNumber"];
                if (pageNumber < 1)
                {
                    throw new Exception();
                }
            }
            catch
            {
                context.ActionArguments["pageNumber"] = 1;
            }
        }
    }
}
