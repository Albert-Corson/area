using System.Collections.Generic;
using System.Linq;
using Area.API.DbContexts;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace Area.API.Repositories
{
    public sealed class WidgetRepository
    {
        private readonly AreaDbContext _database;

        public WidgetRepository(AreaDbContext database)
        {
            _database = database;
        }

        public bool WidgetExists(int widgetId)
        {
            return _database.Widgets.SingleOrDefault(model => model.Id == widgetId) != null;
        }

        public IEnumerable<WidgetModel> GetWidgets(bool includeChildren = false)
        {
            return includeChildren
                ? _database.Widgets.AsNoTracking()
                    .Include(model => model.Params).ThenInclude(model => model.Enums).ThenInclude(model => model.Enum)
                    .Include(model => model.Service)
                : _database.Widgets.AsNoTracking();
        }

        public IEnumerable<WidgetModel> GetWidgetsByService(int serviceId, bool includeChildren = false)
        {
            var queryable = _database.Widgets.AsNoTracking()
                .Where(model => model.ServiceId == serviceId);

            return includeChildren
                ? queryable
                    .Include(model => model.Params).ThenInclude(model => model.Enums).ThenInclude(model => model.Enum)
                    .Include(model => model.Service)
                : queryable;
        }

        public WidgetModel? GetWidget(int widgetId, bool includeParams = false)
        {
            var queryable = includeParams
                ? _database.Widgets.AsNoTracking()
                    .Include(model => model.Params).ThenInclude(model => model.Enums).ThenInclude(model => model.Enum)
                : _database.Widgets.AsNoTracking();

            return queryable.FirstOrDefault(model => model.Id == widgetId);
        }

        public IEnumerable<WidgetModel> GetUserWidgets(int userId, bool includeChildren = false)
        {
            var queryable = _database.Set<UserWidgetModel>()
                .AsNoTracking()
                .Where(model => model.UserId == userId);

            queryable = includeChildren
                ? queryable.Include(model => model.Widget)
                    .ThenInclude(model => model!.Service)
                    .Include(model => model.Widget)
                    .ThenInclude(model => model!.Params).ThenInclude(model => model.Enums).ThenInclude(model => model.Enum)
                : queryable;

            return queryable.Select(model => model.Widget!);
        }

        public IEnumerable<WidgetModel> GetUserWidgetsByService(int userId, int serviceId)
        {
            return GetUserWidgets(userId, true)
                .Where(model => model.ServiceId == serviceId);
        }
    }
}