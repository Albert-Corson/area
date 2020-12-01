using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dashboard.API.Models.Response;
using Dashboard.API.Repositories;
using Dashboard.API.Services;
using Microsoft.AspNetCore.Http;

namespace Dashboard.API.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DatabaseRepository database)
        {
            if (context.Request.Headers.ContainsKey("Authorization") && context.User != null) {
                var userId = AuthService.GetUserIdFromPrincipal(context.User);

                if (userId == null) {
                    await HandlerUnauthorized(context);
                    return;
                }
                var user = database.Users.FirstOrDefault(model => model.Id == userId);
                if (user == null) {
                    await HandlerUnauthorized(context);
                    return;
                }
            }
            await _next(context);
        }

        private static Task HandlerUnauthorized(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return context.Response.WriteAsync(StatusModel.Failed("Unauthorized").ToString());
        }
    }
}
