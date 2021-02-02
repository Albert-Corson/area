using System;
using Microsoft.AspNetCore.Http;

namespace Area.API.Services.Services
{
    public interface IServiceService
    {
        public Uri? SignIn(int userId);

        public int? GetUserIdFromCallbackContext(HttpContext context);

        public string? HandleSignInCallback(HttpContext context);
    }
}