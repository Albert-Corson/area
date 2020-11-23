using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Dashboard.API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Middlewares
{
    public sealed class HttpExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try {
                await _next.Invoke(context);
            } catch (HttpException exception) {
                await HandleExceptionAsync(
                    context, GenerateProblemDetails(context, exception.StatusCode, exception.Message), exception);
            } catch (Exception exception) {
                await HandleExceptionAsync(
                    context, GenerateProblemDetails(context, HttpStatusCode.InternalServerError, exception.Message),
                    exception);
            }
        }

        private static ProblemDetails GenerateProblemDetails(HttpContext context, HttpStatusCode code, string message)
        {
            // Machine-readable format for specifying errors in HTTP API responses.
            // Based on https://tools.ietf.org/html/rfc7807.
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails
            return new ProblemDetails {
                Status = (int) code,
                Title = message,
                Instance = context.Request.Path
            };
        }

        private static Task HandleExceptionAsync(HttpContext context, ProblemDetails problemDetails,
            Exception exception)
        {
            // Check if the response status, reason and headers can be modified.
            // If HasStarted is true, they can't.
            if (context.Response.HasStarted)
                return Task.CompletedTask;

            context.Response.Clear();
            context.Response.ContentType = "application/problem+json";

            if (problemDetails.Status != null)
                context.Response.StatusCode = (int) problemDetails.Status;

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
