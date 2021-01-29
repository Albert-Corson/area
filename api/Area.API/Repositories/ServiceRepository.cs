using System.Collections.Generic;
using System.Linq;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Microsoft.EntityFrameworkCore;
using DbContext = Area.API.Database.DbContext;

namespace Area.API.Repositories
{
    public class ServiceRepository : ARepository
    {
        public ServiceRepository(DbContext database) : base(database)
        { }

        public bool ServiceExists(int serviceId)
        {
            return GetService(serviceId) != null;
        }

        public IEnumerable<ServiceModel> GetServices(bool includeChildren = false, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Services.AsNoTracking() : Database.Services.AsQueryable();

            return includeChildren
                ? queryable.Include(model => model.Widgets)
                    .ThenInclude(model => model.Params)
                : queryable;
        }

        public List<ServiceModel> GetServicesByUser(int userId)
        {
            var services = Database.Set<UserWidgetModel>()
                .AsNoTracking()
                .Where(model => model.UserId == userId)
                .Include(model => model.Widget).ThenInclude(model => model!.Service)
                .Select(model => model.Widget!)
                .Select(model => model.Service);

            List<ServiceModel> filteredServices = new List<ServiceModel>();
            foreach (var service in services) {
                if (!filteredServices.Exists(model => model.Id == service!.Id))
                    filteredServices.Add(service!);
            }

            return filteredServices;
        }

        public ServiceModel? GetService(int serviceId, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Services.AsNoTracking() : Database.Services.AsQueryable();

            return queryable.FirstOrDefault(model => model.Id == serviceId);
        }
    }
}