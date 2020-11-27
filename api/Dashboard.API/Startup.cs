using System.Text;
using Dashboard.API.Constants;
using Dashboard.API.Middlewares;
using Dashboard.API.Services;
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
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[JwtConstants.SecretKeyName]));
            var tokenValidationParameters = new TokenValidationParameters {
                ValidIssuer = _configuration[JwtConstants.ValidIssuer],
                ValidAudience = _configuration[JwtConstants.ValidAudience],
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(config => {
                    config.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddSingleton(_configuration);
            services.AddSingleton(tokenValidationParameters);
            services.AddSingleton<AuthService>();

            services
                .AddControllers()
                .AddNewtonsoftJson();
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
