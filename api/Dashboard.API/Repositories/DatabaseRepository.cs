using Dashboard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.API.Repositories
{
    public class DatabaseRepository : DbContext
    {
        public DatabaseRepository(DbContextOptions<DatabaseRepository> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many between users and widgets
            {
                modelBuilder.Entity<UserWidgetModel>()
                    .HasKey(model => new {
                        model.UserId,
                        model.WidgetId
                    });
                modelBuilder.Entity<UserWidgetModel>()
                    .HasOne(userWidgetModel => userWidgetModel.User)
                    .WithMany(userModel => userModel!.Widgets)
                    .HasForeignKey(userWidgetModel => userWidgetModel.UserId);
                modelBuilder.Entity<UserWidgetModel>()
                    .HasOne(userWidget => userWidget.Widget)
                    .WithMany(widget => widget!.Users)
                    .HasForeignKey(userWidget => userWidget.WidgetId);
            }

            // Many-to-many between users and services
            {
                modelBuilder.Entity<UserServiceModel>()
                    .HasKey(model => new {
                        model.UserId,
                        model.ServiceId
                    });
                modelBuilder.Entity<UserServiceModel>()
                    .HasOne(userWidgetModel => userWidgetModel.User)
                    .WithMany(userModel => userModel!.Services)
                    .HasForeignKey(userWidgetModel => userWidgetModel.UserId);
                modelBuilder.Entity<UserServiceModel>()
                    .HasOne(userWidget => userWidget.Service)
                    .WithMany(widget => widget!.Users)
                    .HasForeignKey(userWidget => userWidget.ServiceId);
            }
        }

        public DbSet<UserModel> Users { get; set; } = null!;

        public DbSet<ServiceModel> Services { get; set; } = null!;

        public DbSet<WidgetModel> Widgets { get; set; } = null!;
    }
}
