using System;
using System.Text;
using Area.API.Authentication;
using Area.API.Constants;
using Area.API.DbContexts;
using Area.API.Middlewares;
using Area.API.Repositories;
using Area.API.Services;
using Area.API.Services.Services;
using Area.API.Services.Widgets.CatApi;
using Area.API.Services.Widgets.Icanhazdadjoke;
using Area.API.Services.Widgets.Imgur;
using Area.API.Services.Widgets.LoremPicsum;
using Area.API.Services.Widgets.NewsApi;
using Area.API.Services.Widgets.Spotify;
using Area.API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Area.API
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
                .AddScheme<JwtBearerOptions, JwtAuthentication>(JwtBearerDefaults.AuthenticationScheme,
                    options => { options.TokenValidationParameters = tokenValidationParameters; });

            services.AddDbContext<AreaDbContext>(options => {
                string connectionString = $"Host={_configuration[PostgresConstants.HostKeyName] ?? "localhost"};" +
                    $"Port={_configuration[PostgresConstants.PortKeyName] ?? "5432"};" +
                    $"Username={_configuration[PostgresConstants.UserKeyName] ?? "postgres"};" +
                    $"Password={_configuration[PostgresConstants.PasswdKeyName] ?? "postgres"};" +
                    $"Database={_configuration[PostgresConstants.DbKeyName] ?? "area"};";
                options.UseNpgsql(connectionString,
                    builder => { builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
            });

            services.AddSingleton(_configuration);
            services.AddSingleton(tokenValidationParameters);
            services.AddSingleton<AuthUtilities>();

            AddWidgetServices(services);
            AddServiceServices(services);
            AddRepositoryServices(services);
            AddSwaggerServices(services);

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
        }

        private static void AddSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Area API documentation",
                    Version = "1.0.0",
                    Description = "This the documentation of the Area dashboard API",
                });
                options.EnableAnnotations(true, true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitDbContext(app);

            ConfigureSwagger(app);

            app.UseStatusCodePagesWithReExecute(RoutesConstants.Error);

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseMiddleware<HttpExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Area");
                options.RoutePrefix = RoutesConstants.Docs;
                options.DocumentTitle = "Area API";
                options.DefaultModelRendering(ModelRendering.Example);
            });
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
            var dbContext = serviceScope.ServiceProvider.GetService<AreaDbContext>();
            if (dbContext == null)
                throw new NullReferenceException("Can't obtain the DbContext");
            dbContext.Database.Migrate();
        }

        private static void AddRepositoryServices(IServiceCollection services)
        {
            services.AddTransient<UserRepository>();
            services.AddTransient<ServiceRepository>();
            services.AddTransient<WidgetRepository>();
        }
    }
}