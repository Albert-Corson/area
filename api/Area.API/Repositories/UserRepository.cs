using System.Collections.Generic;
using System.Linq;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Area.API.Models.Table.Owned;
using Microsoft.EntityFrameworkCore;
using DbContext = Area.API.Database.DbContext;

namespace Area.API.Repositories
{
    public class UserRepository : ARepository
    {
        public UserRepository(DbContext database) : base(database)
        { }

        public bool UserExists(int? userId = null, string? username = null, string? email = null)
        {
            if (userId == null && username == null && email == null)
                return false;

            return Database.Users
                .AsNoTracking()
                .FirstOrDefault(model =>
                    (userId == null || model.Id == userId.Value)
                    && (username == null || model.Username == username)
                    && (email == null || model.Email == email)
                ) != null;
        }

        public UserModel? GetUser(string username, string passwd, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Users.AsNoTracking() : Database.Users.AsQueryable();

            return queryable.FirstOrDefault(model => model.Username == username && model.Password == passwd);
        }

        public UserModel? GetUser(int userId, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Users.AsNoTracking() : Database.Users.AsQueryable();

            return queryable.FirstOrDefault(model => model.Id == userId);
        }

        public void AddUser(UserModel user)
        {
            Database.Users.Add(user);
        }

        public bool RemoveUser(int userId)
        {
            var user = GetUser(userId);

            if (user == null)
                return false;
            Database.Users.Remove(user);
            return true;
        }

        public IEnumerable<UserWidgetParamModel> GetUserWidgetParams(int userId, int? widgetId = null)
        {
            var widgetParams = Database.Users.AsNoTracking()
                .FirstOrDefault(model => model.Id == userId)
                ?.WidgetParams?.ToList()
                ?? new List<UserWidgetParamModel>();

            return widgetId != null ? widgetParams.Where(model => model.WidgetId == widgetId) : widgetParams;
        }

        public void AddWidgetSubscription(int userId, int widgetId)
        {
            var dbSet = Database.Set<UserWidgetModel>();
            var existingSub = dbSet.SingleOrDefault(model => model.UserId == userId && model.WidgetId == widgetId);

            if (existingSub != null)
                return;

            dbSet.Add(new UserWidgetModel {
                UserId = userId,
                WidgetId = widgetId
            });
        }

        public bool RemoveWidgetSubscription(int userId, int widgetId)
        {
            var dbSet = Database.Set<UserWidgetModel>();

            var subscription = dbSet.SingleOrDefault(model => model.UserId == userId && model.WidgetId == widgetId);
            var userParams = Database.Users
                .SingleOrDefault(model => model.Id == userId)
                ?.WidgetParams
                ?.Where(model => model.WidgetId == widgetId);

            if (userParams != null)
                Database.RemoveRange(userParams);
            if (subscription == null)
                return false;
            dbSet.Remove(subscription);
            return true;
        }

        public bool AddServiceCredentials(int userId, int serviceId, string jsonTokens)
        {
            var user = Database.Users
                .SingleOrDefault(model => model.Id == userId);

            if (user == null)
                return false;

            RemoveServiceCredentials(userId, serviceId);

            user.ServiceTokens!.Add(new UserServiceTokensModel {
                Json = jsonTokens,
                ServiceId = serviceId
            });
            return true;
        }

        public void RemoveServiceCredentials(int userId, int serviceId)
        {
            var user = Database.Users
                .AsNoTracking()
                .Include(model => model.ServiceTokens)
                .FirstOrDefault(model => model.Id == userId);

            var serviceToken = user?.ServiceTokens
                ?.FirstOrDefault(model => model.ServiceId == serviceId);

            if (serviceToken != null)
                Database.Set<UserServiceTokensModel>().Remove(serviceToken);
        }
    }
}