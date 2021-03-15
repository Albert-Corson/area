using System;
using System.Linq;
using Area.API.Attributes;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Repositories;
using Area.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Swan;

namespace Area.API.Filters
{
    public class AuthorizationFilter : IActionFilter
    {
        private readonly UserRepository _userRepository;
        private readonly AuthService _authService;

        public AuthorizationFilter(UserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var validateIpAddress = false;
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor) {
                if (IsAttribute(descriptor, typeof(AllowAnonymousAttribute)) || !IsAttribute(descriptor, typeof(AuthorizeAttribute)))
                    return;

                validateIpAddress = IsAttribute(descriptor, typeof(ValidateIpAddressAttribute));
            }

            if (!context.HttpContext.User.TryGetExpiry(out var expiry)
                || new DateTime(expiry) >= DateTime.UtcNow)
                throw new UnauthorizedHttpException("This token has expired");

            var user = context.HttpContext.User.TryGetUserId(out var userId)
                ? _userRepository.GetUser(userId, asNoTracking: false)
                : null;

            if (user == null)
                throw new UnauthorizedHttpException("The associated user does not exist");

            var ipv4 = validateIpAddress ? context.HttpContext.Connection.RemoteIpAddress.MapToIPv4() : null;
            if (!_authService.ValidateDeviceUse(context.HttpContext.User, user, ipv4).Await())
                throw new ForbiddenHttpException("You need to sign back in");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }

        private static bool IsAttribute(ControllerActionDescriptor descriptor, Type type)
        {
            var methodAttributes = descriptor.MethodInfo.GetCustomAttributes(true);
            var controllerAttributes = descriptor.ControllerTypeInfo.GetCustomAttributes(true);

            return methodAttributes.Any(attribute => attribute.GetType() == type)
                || controllerAttributes.Any(attribute => attribute.GetType() == type);
        }
    }
}