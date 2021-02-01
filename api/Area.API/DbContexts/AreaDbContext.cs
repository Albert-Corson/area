using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace Area.API.DbContexts
{
    public class AreaDbContext : DbContext
    {
        public AreaDbContext(DbContextOptions<AreaDbContext> options)
            : base(options)
        { }

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