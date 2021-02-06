using Area.API.Exceptions.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Area.API.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;
            throw new BadRequestHttpException("Malformed body or query parameter(s).");
        }
    }
}