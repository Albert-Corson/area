using System.Collections.Generic;
using System.Linq;
using Area.API.DbContexts;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace Area.API.Repositories
{
    public sealed class ServiceRepository
    {
        private readonly AreaDbContext _database;

        public ServiceRepository(AreaDbContext database)
        {
            _database = database;
        }

        public bool ServiceExists(int serviceId)
        {
            return GetService(serviceId) != null;
        }

        public IEnumerable<ServiceModel> GetServices(bool includeWidgets = false)
        {
            return includeWidgets
                ? _database.Services.AsNoTracking()
                    .Include(model => model.Widgets)
                : _database.Services.AsNoTracking();
        }

        public List<ServiceModel> GetServicesByUser(int userId)
        {
            var services = _database.Set<UserWidgetModel>()
                .AsNoTracking()
                .Where(model => model.UserId == userId)
                .Include(model => model.Widget)
                .ThenInclude(model => model!.Service)
                .Select(model => model.Widget!)
                .Select(model => model.Service);

            List<ServiceModel> filteredServices = new List<ServiceModel>();
            foreach (var service in services)
                if (!filteredServices.Exists(model => model.Id == service!.Id))
                    filteredServices.Add(service!);

            return filteredServices;
        }

        public ServiceModel? GetService(int serviceId)
        {
            return _database.Services.AsNoTracking()
                .FirstOrDefault(model => model.Id == serviceId);
        }
    }
}