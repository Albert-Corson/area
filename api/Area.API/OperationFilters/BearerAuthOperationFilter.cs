using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Area.API.OperationFilters
{
    public class BearerAuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.ApiDescription.CustomAttributes().ToList();
            var noAuthRequired = attributes.Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));
            var authRequired = attributes.Any(attr => attr.GetType() == typeof(AuthorizeAttribute));

            if (noAuthRequired || !authRequired)
                return;
            
            operation.Security = new List<OpenApiSecurityRequirement> {
                new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[0]
                    }
                }
            };
        }
    }
}