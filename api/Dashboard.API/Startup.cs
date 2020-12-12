using System;
using System.Text;
using Dashboard.API.Authentication;
using Dashboard.API.Constants;
using Dashboard.API.Middlewares;
using Dashboard.API.Repositories;
using Dashboard.API.Services;
using Dashboard.API.Services.Services;
using Dashboard.API.Services.Widgets.CatApi;
using Dashboard.API.Services.Widgets.Icanhazdadjoke;
using Dashboard.API.Services.Widgets.Imgur;
using Dashboard.API.Services.Widgets.LoremPicsum;
using Dashboard.API.Services.Widgets.NewsApi;
using Dashboard.API.Services.Widgets.Spotify;
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("05lENTWIKCwLNyA4APIZ8odlh848RQZb"));
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

            AddWidgetServices(services);
            AddServiceServices(services);

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
            InitDbContext(app);

            app.UseStatusCodePagesWithReExecute(RoutesConstants.Default.Error);

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseMiddleware<HttpExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void AddWidgetServices(IServiceCollection services)
        {
            services.AddScoped<ImgurGalleryWidgetService>();
            services.AddScoped<ImgurFavoritesWidgetService>();
            services.AddScoped<ImgurUploadsWidgetService>();
            services.AddScoped<ImgurGallerySearchWidgetService>();
            services.AddScoped<LoremPicsumRandomImageService>();
            services.AddScoped<SpotifyFavoriteArtistsWidgetService>();
            services.AddScoped<SpotifyFavoriteTracksWidgetService>();
            services.AddScoped<SpotifyHistoryWidgetService>();
            services.AddScoped<NewsApiTopHeadlinesWidgetService>();
            services.AddScoped<NewsApiSearchWidgetService>();
            services.AddScoped<CatApiRandomImagesWidgetService>();
            services.AddScoped<IcanhazdadjokeRandomJokeWidgetService>();
            services.AddScoped<WidgetManagerService>();
        }

        private static void AddServiceServices(IServiceCollection services)
        {
            services.AddScoped<ImgurServiceService>();
            services.AddScoped<SpotifyServiceService>();
            services.AddScoped<ServiceManagerService>();
        }

        private static void InitDbContext(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<DatabaseRepository>();
            if (dbContext == null)
                throw new NullReferenceException("Can't obtain the DbContext");
            dbContext.Database.Migrate();
        }
    }
}
