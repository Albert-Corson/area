using System;
using Area.API.Models;
using Microsoft.AspNetCore.Http;

namespace Area.API.Services.Services
{
    public interface IServiceService
    {
        public Uri? SignIn(string state);

        public string? HandleSignInCallback(HttpContext context);
    }
}