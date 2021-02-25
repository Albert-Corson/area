using System;
using Area.API.Constants;
using Area.API.Filters;
using Area.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Area.API.Installers
{
    public static class SwaggerInstaller
    {
        public static IApplicationBuilder ConfigureAreaSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Area");
                options.RoutePrefix = RouteConstants.Docs;
                options.DocumentTitle = "Area API";
                options.DefaultModelRendering(ModelRendering.Example);
                options.DefaultModelExpandDepth(int.MaxValue);
            });

            return app;
        }

        public static IServiceCollection AddAreaSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Area API documentation",
                    Version = "1.0.0",
                    Description = @"## This is the documentation of the Area dashboard API.
### All endpoints use the same base data scheme as response, **even for non-successful HTTP status codes** (see `Status`)."
                });
                options.EnableAnnotations(true, true);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    Scheme = "bearer",
                    BearerFormat = "Jwt",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });
                options.OperationFilter<BearerAuthOperationFilter>();
                options.CustomSchemaIds(BuildReadableTypeName);
                options.DescribeAllEnumsAsStrings();
            });

            return services;
        }

        private static string BuildReadableTypeName(Type type)
        {
            var strip = new Func<string, string>(name => {
                name = name.Replace("`1", "");
                return name.EndsWith("Model") ? name.Substring(0, name.Length - 5) : name;
            });
            var abtType = typeof(AboutDotJsonModel);
            var typeName = strip(type.Name);
            string? genericTypeNames = null;

            if (abtType.FullName != type.FullName && type.FullName?.StartsWith(abtType.FullName!) == true)
                typeName = strip(abtType.Name) + "." + strip(type.Name);

            foreach (var genericType in type.GenericTypeArguments) {
                var genericTypeName = BuildReadableTypeName(genericType);
                if (genericTypeNames == null)
                    genericTypeNames = genericTypeName;
                else
                    genericTypeNames += ", " + genericTypeName;
            }

            if (genericTypeNames != null)
                return typeName + "<" + genericTypeNames + ">";
            return typeName;
        }
    }
}