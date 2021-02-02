using System;
using System.Net;
using System.Threading.Tasks;
using Area.API.Exceptions;
using Area.API.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Area.API.Middlewares
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
                await HandleExceptionAsync(context, GenerateResponseBody(exception.Message), exception.StatusCode);
            } catch (Exception exception) {
                await HandleExceptionAsync(context, GenerateResponseBody(exception.Message),
                    HttpStatusCode.InternalServerError);
            }
        }

        private static StatusModel GenerateResponseBody(string message)
        {
            return new StatusModel {
                Successful = false,
                Error = message
            };
        }

        private static Task HandleExceptionAsync(HttpContext context, StatusModel responseBody,
            HttpStatusCode statusCode)
        {
            // Check if the response status, reason and headers can be modified.
            // If HasStarted is true, they can't.
            if (context.Response.HasStarted)
                return Task.CompletedTask;

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(responseBody));
        }
    }
}