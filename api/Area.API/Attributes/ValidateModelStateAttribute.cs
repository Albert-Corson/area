using System;
using System.Linq;
using Area.API.Exceptions.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Area.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private readonly bool _activated;

        public ValidateModelStateAttribute()
            : this(true)
        { }

        public ValidateModelStateAttribute(bool activated)
        {
            _activated = activated;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_activated || context.ModelState.IsValid)
                return;

            string str = context.ModelState.Values
                .SelectMany(entry => entry.Errors)
                .Aggregate("", (current, it) => current + it.ErrorMessage + "; ");

            throw new BadRequestHttpException(str);
        }
    }
}