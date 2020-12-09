using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.API.Repositories
{
    public class DatabaseRepository : DbContext
    {
        public DatabaseRepository(DbContextOptions<DatabaseRepository> options)
            : base(options)
        {}

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

            // A user owns many service tokens
            {
                modelBuilder.Entity<UserModel>()
                    .OwnsMany(model => model.ServiceTokens);
            }

            // A user owns many widget parameters
            {
                modelBuilder.Entity<UserModel>()
                    .OwnsMany(model => model.WidgetParams);
            }

            // A widget owns many default parameters
            {
                modelBuilder.Entity<WidgetModel>()
                    .OwnsMany(model => model.Params);
            }
        }

        public DbSet<UserModel> Users { get; set; } = null!;

        public DbSet<ServiceModel> Services { get; set; } = null!;

        public DbSet<WidgetModel> Widgets { get; set; } = null!;
    }
}
