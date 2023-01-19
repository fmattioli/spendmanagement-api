using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Spents.Infra.CrossCutting.Middlewares.Validators
{
    public class CustomValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();

                context.Result = new JsonResult(new
                {
                    Code = 400,
                    Message = "One or more validation errors occurred.",
                    Errors = errors
                })
                {
                    StatusCode = 200
                };
            }
        }
    }
}
