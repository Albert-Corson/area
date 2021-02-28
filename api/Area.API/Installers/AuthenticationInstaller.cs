using System;
using System.Text;
using Area.API.Authentication;
using Area.API.Constants;
using Area.API.DbContexts;
using Area.API.Models.Table;
using Area.API.Services;
using IpData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
                ValidAudience = configuration[AuthConstants.ValidAudience],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[AuthConstants.SecretKeyName])),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, JwtAuthentication>(JwtBearerDefaults.AuthenticationScheme,
                    options => { options.TokenValidationParameters = tokenValidationParameters; });
            services.AddSingleton(tokenValidationParameters);

            services.AddDetection();
            services.AddScoped(provider => new IpDataClient(configuration[AuthConstants.IpData.Key]));
            services.AddScoped<AuthService>();

            services.AddIdentity<UserModel, UserModel.RoleModel>(options => {
                    options.Password = new PasswordOptions {
                        RequireDigit = true,
                        RequiredLength = 8,
                        RequireLowercase = true,
                        RequireUppercase = true,
                        RequireNonAlphanumeric = true
                    };
                    options.User = new UserOptions {
                        RequireUniqueEmail = true
                    };
                })
                .AddEntityFrameworkStores<AreaDbContext>();

            return services;
        }
    }
}