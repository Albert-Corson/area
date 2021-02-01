using System.Collections.Generic;
using System.Linq;
using Area.API.DbContexts;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace Area.API.Repositories
{
    public class WidgetRepository : ARepository
    {
        public WidgetRepository(AreaDbContext database) : base(database)
        { }

        public bool WidgetExists(int widgetId)
        {
            return Database.Widgets.SingleOrDefault(model => model.Id == widgetId) != null;
        }

        public IEnumerable<WidgetModel> GetWidgets(bool includeChildren = false, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Widgets.AsNoTracking() : Database.Widgets.AsQueryable();

            return includeChildren
                ? queryable.Include(model => model.Params)
                    .Include(model => model.Service)
                : queryable;
        }

        public IEnumerable<WidgetModel> GetWidgetsByService(int serviceId, bool includeChildren = false,
            bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Widgets.AsNoTracking() : Database.Widgets.AsQueryable();
            queryable = queryable.Where(model => model.ServiceId == serviceId);

            return includeChildren
                ? queryable
                    .Include(model => model.Params)
                    .Include(model => model.Service)
                : queryable;
        }

        public WidgetModel? GetWidget(int widgetId, bool includeParams = false, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Widgets.AsNoTracking() : Database.Widgets.AsQueryable();

            queryable = includeParams
                ? queryable.Include(model => model.Params)
                : queryable;

            return queryable.FirstOrDefault(model => model.Id == widgetId);
        }

        public IEnumerable<WidgetModel> GetUserWidgets(int userId, bool includeChildren = false,
            bool asNoTracking = true)
        {
            var queryable = asNoTracking
                ? Database.Set<UserWidgetModel>().AsNoTracking()
                : Database.Set<UserWidgetModel>().AsQueryable();

            queryable = queryable.Where(model => model.UserId == userId);

            queryable = includeChildren
                ? queryable.Include(model => model.Widget).ThenInclude(model => model!.Service)
                    .Include(model => model.Widget).ThenInclude(model => model!.Params)
                : queryable;


            return queryable.Select(model => model.Widget!);
        }

        public IEnumerable<WidgetModel> GetUserWidgetsByService(int userId, int serviceId, bool asNoTracking = true)
        {
            return GetUserWidgets(userId, asNoTracking: asNoTracking)
                .Where(model => model.ServiceId == serviceId);
        }
    }
}