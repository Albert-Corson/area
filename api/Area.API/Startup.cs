using System;
using System.Net;
using System.Text;
using Area.API.Attributes;
using Area.API.Authentication;
using Area.API.Constants;
using Area.API.DbContexts;
using Area.API.Filters;
using Area.API.Middlewares;
using Area.API.Models;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
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

            services.AddMvc(options => {
                options.Filters.Add(new ValidateModelStateAttribute());
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.OK));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.Unauthorized));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.BadRequest));
                options.Filters.Add(new SwaggerResponseAttribute((int) HttpStatusCode.InternalServerError));
            });
        }

        private static void AddSwaggerServices(IServiceCollection services)
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
                    Scheme = "Bearer",
                    BearerFormat = "Jwt",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });
                options.OperationFilter<BearerAuthOperationFilter>();
                options.CustomSchemaIds(BuildReadableTypeName);
            });
        }

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

        private static void InitDbContext(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetService<AreaDbContext>();
            if (dbContext == null)
                throw new NullReferenceException("Can't obtain the DbContext");
            dbContext.Database.Migrate();
        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Area");
                options.RoutePrefix = RoutesConstants.Docs;
                options.DocumentTitle = "Area API";
                options.DefaultModelRendering(ModelRendering.Example);
                options.DefaultModelExpandDepth(int.MaxValue);
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

        private static void AddRepositoryServices(IServiceCollection services)
        {
            services.AddTransient<UserRepository>();
            services.AddTransient<ServiceRepository>();
            services.AddTransient<WidgetRepository>();
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