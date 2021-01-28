using System.Collections.Generic;
using System.Linq;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;
using DbContext = Area.API.Database.DbContext;

namespace Area.API.Repositories
{
    public class ServiceRepository
    {
        private readonly DbContext _database;

        public ServiceRepository(DbContext database)
        {
            _database = database;
        }

        public bool ServiceExists(int serviceId)
        {
            return GetService(serviceId) != null;
        }

        public IEnumerable<ServiceModel> GetServices(bool asNoTracking = true)
        {
            return asNoTracking ? _database.Services.AsNoTracking() : _database.Services.AsQueryable();
        }

        public IEnumerable<ServiceModel> GetServicesWithChildren(bool asNoTracking = true)
        {
            var queryable = asNoTracking ? _database.Services.AsNoTracking() : _database.Services.AsQueryable();

            return queryable
                .Include(model => model.Widgets)
                .ThenInclude(model => model.Params);
        }

        public List<ServiceModel> GetServicesByUser(int userId)
        {
            var userWidgets = _database.Set<UserWidgetModel>()
                .AsNoTracking()
                .Where(model => model.UserId == userId)
                .Include(model => model.Widget)
                .ThenInclude(model => model!.Service);

            List<ServiceModel> services = new List<ServiceModel>();
            foreach (var userWidget in userWidgets) {
                if (services.Contains(userWidget.Widget!.Service!))
                    continue;
                services.Add(userWidget.Widget.Service!);
            }

            return services;
        }

        public ServiceModel? GetService(int serviceId)
        {
            return _database.Services
                .AsNoTracking()
                .FirstOrDefault(model => model.Id == serviceId);
        }
    }
}