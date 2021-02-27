using System;
using Area.API.Constants;
using Area.API.DbContexts;
using Area.API.Models.Table;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Area.API.Installers
{
    public static class DbContextInstaller
    {
        public static IApplicationBuilder ConfigureAreaDbContext(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetService<AreaDbContext>();
            if (dbContext == null)
                throw new NullReferenceException("Can't obtain the DbContext");
            dbContext.Database.Migrate();

            return app;
        }

        public static IServiceCollection AddAreaDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AreaDbContext>(options => {
                string connectionString = $"Host={configuration[PostgresConstants.HostKeyName] ?? "localhost"};" +
                    $"Port={configuration[PostgresConstants.PortKeyName] ?? "5432"};" +
                    $"Username={configuration[PostgresConstants.UserKeyName] ?? "postgres"};" +
                    $"Password={configuration[PostgresConstants.PasswdKeyName] ?? "postgres"};" +
                    $"Database={configuration[PostgresConstants.DbKeyName] ?? "area"};";
                options.UseNpgsql(connectionString,
                    builder => { builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
            });

            return services;
        }
    }
}