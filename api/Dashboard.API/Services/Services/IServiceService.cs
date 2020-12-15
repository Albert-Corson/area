using System;
using Dashboard.API.Models.Table;
using Microsoft.AspNetCore.Http;

namespace Dashboard.API.Services.Services
{
    public interface IServiceService
    {
        public string Name { get; }

        public Uri? SignIn(HttpContext context, int userId);

        public int? GetUserIdFromCallbackContext(HttpContext context);

        public bool HandleSignInCallback(HttpContext context, int serviceId, UserModel user);
    }
}
