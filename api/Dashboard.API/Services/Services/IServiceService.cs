using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Services.Services
{
    public interface IServiceService
    {
        public string Name { get; }

        string? LogIn(HttpContext context);

        void HandleLogInCallback(HttpContext context);
    }
}