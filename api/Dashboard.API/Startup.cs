using System.Text;
using Dashboard.API.Authentication;
using Dashboard.API.Constants;
using Dashboard.API.Middlewares;
using Dashboard.API.Repositories;
using Dashboard.API.Services;
using Dashboard.API.Services.Widgets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
                .AddScheme<JwtBearerOptions, JwtAuthentication>(JwtBearerDefaults.AuthenticationScheme, options => {
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddDbContext<DatabaseRepository>(options => {
                string connectionString = $"Host={_configuration[PostgresConstants.HostKeyName] ?? "localhost"};" +
                                          $"Port={_configuration[PostgresConstants.PortKeyName] ?? "5432"};" +
                                          $"Username={_configuration[PostgresConstants.UserKeyName] ?? "postgres"};" +
                                          $"Password={_configuration[PostgresConstants.PasswdKeyName] ?? "postgres"};" +
                                          $"Database={_configuration[PostgresConstants.DbKeyName] ?? "dashboard"};";
                options.UseNpgsql(connectionString, builder => {
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });

            services.AddSingleton(_configuration);
            services.AddSingleton(tokenValidationParameters);
            services.AddSingleton<AuthService>();

            services.AddScoped<ImgurGalleryWidgetService>();
            services.AddScoped<WidgetManagerService>();

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute(RoutesConstants.Default.Error);

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseMiddleware<HttpExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
