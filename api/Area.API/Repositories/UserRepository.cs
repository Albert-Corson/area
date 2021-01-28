using System.Collections.Generic;
using System.Linq;
using Area.API.Common;
using Area.API.Constants;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Area.API.Models.Table.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbContext = Area.API.Database.DbContext;

namespace Area.API.Repositories
{
    public class UserRepository
    {
        private readonly DbContext _database;
        private readonly IConfiguration _configuration;

        public UserRepository(DbContext database, IConfiguration configuration)
        {
            _database = database;
            _configuration = configuration;
        }

        public bool UserExists(int? userId = null, string? username = null, string? email = null)
        {
            if (userId == null && username == null && email == null)
                return false;

            return _database.Users
                .AsNoTracking()
                .FirstOrDefault(model =>
                    (userId == null || model.Id == userId.Value)
                    && (username == null || model.Username == username)
                    && (email == null || model.Email == email)
                ) != null;
        }

        public UserModel? GetUser(string username, string clearPasswd)
        {
            var encryptedPasswd = Encryptor.Encrypt(_configuration[JwtConstants.SecretKeyName], clearPasswd);

            return _database.Users
                .AsNoTracking()
                .FirstOrDefault(model => model.Username == username && model.Password == encryptedPasswd);
        }

        public UserModel? GetUser(int userId)
        {
            return _database.Users
                .AsNoTracking()
                .FirstOrDefault(model => model.Id == userId);
        }

        public bool AddUser(string username, string email, string clearPasswd)
        {
            var encryptedPasswd = Encryptor.Encrypt(_configuration[JwtConstants.SecretKeyName], clearPasswd);

            if (encryptedPasswd == null)
                return false;
            _database.Users.Add(new UserModel {
                Username = username,
                Password = encryptedPasswd,
                Email = email
            });
            _database.SaveChanges();
            return true;
        }

        public bool RemoveUser(int userId)
        {
            var user = GetUser(userId);

            if (user == null)
                return false;
            _database.Users.Remove(user);
            _database.SaveChanges();
            return true;
        }

        public IEnumerable<UserWidgetParamModel> GetUserWidgetParams(int userId, int? widgetId = null)
        {
            var widgetParams = _database.Users
                .Where(model => model.Id == userId)
                .Include(model => model.WidgetParams)
                .SelectMany(model => model.WidgetParams);

            return widgetId != null ? widgetParams.Where(model => model.WidgetId == widgetId) : widgetParams;
        }

        public bool AddWidgetParam(int userId, int widgetId, string name, string type, string value)
        {
            var user = _database.Users
                .SingleOrDefault(model => model.Id == userId);

            if (user == null)
                return false;

            user.WidgetParams!.Add(new UserWidgetParamModel {
                Name = name,
                Type = type,
                Value = value,
                WidgetId = widgetId
            });
            _database.SaveChanges();
            return true;
        }

        public bool RemoveWidgetSubscription(int userId, int widgetId)
        {
            var dbSet = _database.Set<UserWidgetModel>();

            var subscription = dbSet.SingleOrDefault(model => model.UserId == userId && model.WidgetId == widgetId);
            var userParams = _database.Users
                .SingleOrDefault(model => model.Id == userId)
                ?.WidgetParams
                ?.Where(model => model.WidgetId == widgetId);

            if (userParams != null)
                _database.RemoveRange(userParams);
            if (subscription == null)
                return false;
            dbSet.Remove(subscription);
            _database.SaveChanges();
            return true;
        }

        public void AddWidgetSubscription(int userId, int widgetId)
        {
            var dbSet = _database.Set<UserWidgetModel>();
            var existingSub = dbSet.SingleOrDefault(model => model.UserId == userId && model.WidgetId == widgetId);

            if (existingSub == null)
                return;

            dbSet.Add(new UserWidgetModel {
                UserId = userId,
                WidgetId = widgetId
            });
            _database.SaveChanges();
        }

        public bool RemoveServiceCredentials(int userId, int serviceId)
        {
            var user = _database.Users
                .AsNoTracking()
                .Include(model => model.ServiceTokens)
                .FirstOrDefault(model => model.Id == userId);

            var serviceToken = user?.ServiceTokens
                ?.FirstOrDefault(model => model.ServiceId == serviceId);

            if (serviceToken == null)
                return false;
            _database.Set<UserServiceTokensModel>().Remove(serviceToken);
            _database.SaveChanges();
            return true;
        }

        public bool AddServiceCredentials(int userId, int serviceId, string jsonTokens)
        {
            var user = _database.Users
                .SingleOrDefault(model => model.Id == userId);

            if (user == null)
                return false;

            RemoveServiceCredentials(userId, serviceId);

            user.ServiceTokens!.Add(new UserServiceTokensModel {
                Json = jsonTokens,
                ServiceId = serviceId
            });
            _database.SaveChanges();
            return true;
        }
    }
}