using Microsoft.AspNetCore.Http;

namespace Dashboard.API.Services.Services
{
    public interface IServiceService
    {
        public string Name { get; }

        public string? SignIn(HttpContext context);

        public void HandleSignInCallback(HttpContext context, int serviceId);
    }
}
