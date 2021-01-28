using System.Collections.Generic;
using System.Linq;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;
using DbContext = Area.API.Database.DbContext;

namespace Area.API.Repositories
{
    public class WidgetRepository
    {
        private readonly DbContext _database;

        public WidgetRepository(DbContext database)
        {
            _database = database;
        }

        public bool WidgetExists(int widgetId)
        {
            return _database.Widgets.SingleOrDefault(model => model.Id == widgetId) != null;
        }

        public IEnumerable<WidgetModel> GetWidgets()
        {
            return _database.Widgets
                .AsNoTracking()
                .Include(model => model.Params)
                .Include(model => model.Service);
        }

        public IEnumerable<WidgetModel> GetWidgetsByService(int serviceId)
        {
            return _database.Widgets
                .AsNoTracking()
                .Where(model => model.ServiceId == serviceId)
                .Include(model => model.Params)
                .Include(model => model.Service);
        }

        public WidgetModel? GetWidgetWithParams(int widgetId)
        {
            return _database.Widgets
                .AsNoTracking()
                .Include(model => model.Params)
                .SingleOrDefault(model => model.Id == widgetId);
        }

        public IEnumerable<WidgetModel> GetUserWidgets(int userId)
        {
            return _database.Set<UserWidgetModel>()
                .Where(model => model.UserId == userId)
                .Include(model => model.Widget)
                .Select(model => model.Widget!)
                .Include(model => model!.Service)
                .Include(model => model!.Params);
        }

        public IEnumerable<WidgetModel> GetUserWidgetsByService(int userId, int serviceId)
        {
            return _database.Set<UserWidgetModel>()
                .Where(model => model.UserId == userId)
                .Include(model => model.Widget)
                .Select(model => model.Widget!)
                .Where(model => model.ServiceId == serviceId)
                .Include(model => model!.Service)
                .Include(model => model!.Params);
        }
    }
}