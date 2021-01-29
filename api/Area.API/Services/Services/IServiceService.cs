using System;
using Microsoft.AspNetCore.Http;

namespace Area.API.Services.Services
{
    public interface IServiceService
    {
        public string Name { get; }

        public Uri? SignIn(HttpContext context, int userId);

        public int? GetUserIdFromCallbackContext(HttpContext context);

        public string? HandleSignInCallback(HttpContext context);
    }
}
