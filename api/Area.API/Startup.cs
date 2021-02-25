using System.Net;
using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Installers;
using Area.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAreaAuthentication(_configuration)
                .AddAreaDbContext(_configuration)
                .AddAreaSwagger()
                .AddAreaRepositories()
                .AddAreaWidgets()
                .AddAreaServices();

            services
                .AddControllers()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            services.AddMvc(options => {
                options.Filters.Add(new ValidateModelStateAttribute());
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.OK));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.Unauthorized));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.BadRequest));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.InternalServerError));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .ConfigureAreaDbContext()
                .ConfigureAreaSwagger()
                .UseStatusCodePagesWithReExecute(RouteConstants.Error)
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseCors()
                .UseMiddleware<HttpExceptionHandlingMiddleware>()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}