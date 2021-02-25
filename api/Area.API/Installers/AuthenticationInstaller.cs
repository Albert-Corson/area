using System.Text;
using Area.API.Authentication;
using Area.API.Constants;
using Area.API.Services;
using IpData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Area.API.Installers
{
    public static class AuthenticationInstaller
    {
        public static IServiceCollection AddAreaAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidAudience = configuration[JwtConstants.ValidAudience],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[JwtConstants.SecretKeyName])),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, JwtAuthentication>(JwtBearerDefaults.AuthenticationScheme,
                    options => { options.TokenValidationParameters = tokenValidationParameters; });

            services.AddSingleton(tokenValidationParameters);

            services.AddDetection();
            services.AddScoped(provider => new IpDataClient(configuration["IpDataKey"]));
            services.AddScoped<AuthService>();

            return services;
        }
    }
}