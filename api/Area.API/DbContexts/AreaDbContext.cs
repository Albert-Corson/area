using System;
using System.Linq;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Area.API.DbContexts
{
    public class AreaDbContext : IdentityDbContext<UserModel, UserModel.RoleModel, int>
    {
        public AreaDbContext(DbContextOptions<AreaDbContext> options)
            : base(options)
        { }

        public DbSet<ServiceModel> Services { get; set; } = null!;

        public DbSet<WidgetModel> Widgets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many between users and widgets
            modelBuilder.Entity<UserWidgetModel>()
                .HasKey(model => new {
                    model.UserId,
                    model.WidgetId
                });
            modelBuilder.Entity<UserWidgetModel>()
                .HasOne(userWidgetModel => userWidgetModel.User)
                .WithMany(userModel => userModel.Widgets)
                .HasForeignKey(userWidgetModel => userWidgetModel.UserId);
            modelBuilder.Entity<UserWidgetModel>()
                .HasOne(userWidget => userWidget.Widget)
                .WithMany(widget => widget.Users)
                .HasForeignKey(userWidget => userWidget.WidgetId);

            // Many-to-many between params and enums
            modelBuilder.Entity<ParamEnumModel>()
                .HasKey(model => new {
                    model.ParamId,
                    model.EnumId
                });
            modelBuilder.Entity<ParamEnumModel>()
                .HasOne(model => model.Param)
                .WithMany(model => model.Enums)
                .HasForeignKey(model => model.ParamId);

            // A user owns many service tokens
            modelBuilder.Entity<UserModel>()
                .OwnsMany(model => model.ServiceTokens);

            // A user owns many widget parameters
            modelBuilder.Entity<UserModel>()
                .OwnsMany(model => model.WidgetParams);
        }

        public bool SafeSaveChanges()
        {
            bool saveFailed;
            do {
                saveFailed = false;

                try {
                    SaveChanges();
                } catch (DbUpdateConcurrencyException ex) {
                    saveFailed = true;
                    ex.Entries.Single().Reload();
                } catch (ObjectDisposedException) { }
            } while (saveFailed);

            return !saveFailed;
        }
    }
}