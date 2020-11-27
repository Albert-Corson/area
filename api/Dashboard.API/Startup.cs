using System;
using System.Text;
using Dashboard.API.Constants;
using Dashboard.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config => {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[JwtConstants.SecretKeyName]));

                    config.TokenValidationParameters = new TokenValidationParameters {
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = Configuration[JwtConstants.ValidIssuer],
                        ValidAudience = Configuration[JwtConstants.ValidAudience],
                        IssuerSigningKey = key
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute("/Error");

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<HttpExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
