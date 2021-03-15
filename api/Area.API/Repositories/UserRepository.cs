using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Area.API.DbContexts;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Area.API.Models.Table.Owned;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swan;

namespace Area.API.Repositories
{
    public sealed class UserRepository : IDisposable
    {
        private readonly AreaDbContext _database;
        private readonly WidgetRepository _widgetRepository;
        private readonly UserManager<UserModel> _userManager;

        public UserRepository(AreaDbContext database, WidgetRepository widgetRepository, UserManager<UserModel> userManager)
        {
            _database = database;
            _widgetRepository = widgetRepository;
            _userManager = userManager;
        }

        public void Dispose()
        {
            _database.SafeSaveChanges();
        }

        public bool UserExists(int? userId = null, string? username = null, string? email = null, UserModel.UserType? type = null)
        {
            return _database.Users.FirstOrDefault(model =>
                (model.UserName == username || model.Email == email || (userId != null && model.Id == userId.Value))
                && (type == null || model.Type == type.Value)) != null;
        }

        public UserModel? GetUser(int? userId = null, string? username = null, string? email = null,
            string? passwd = null, UserModel.UserType? type = null, bool includeChildren = false, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? _database.Users.AsNoTracking() : _database.Users.AsQueryable();
            queryable = includeChildren
                ? queryable
                    .Include(model => model.WidgetParams).ThenInclude(model => model.Param).ThenInclude(model => model.Enums).ThenInclude(model => model.Enum)
                : queryable;

            if (userId == null && username == null && email == null && passwd == null)
                throw new ArgumentException("At least one non-null argument expected");

            var user = queryable.FirstOrDefault(model =>
                (userId == null || model.Id == userId)
                && (username == null || model.UserName == username)
                && (email == null || model.Email == email)
                && (type == null || model.Type == type)
            );

            if (user == null || passwd == null)
                return user;

            return !_userManager.CheckPasswordAsync(user, passwd).Await() ? null : user;
        }

        public Task<IdentityResult> AddUserAsync(UserModel user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> AddUserAsync(UserModel user)
        {
            return _userManager.CreateAsync(user);
        }

        public Task<IdentityResult> AddUserClaimAsync(UserModel user, Claim claim)
        {
            return _userManager.AddClaimAsync(user, claim);
        }

        public Task<IList<Claim>> GetUserClaimsAsync(UserModel user)
        {
            return _userManager.GetClaimsAsync(user);
        }

        public IdentityUserClaim<int>? GetUserClaim(int userId, string claimType)
        {
            return _database.UserClaims.FirstOrDefault(claim => claim.UserId == userId && claim.ClaimType == claimType);
        }

        public Task<IdentityResult> RemoveUserClaimsAsync(UserModel user, IEnumerable<Claim> claims)
        {
            return _userManager.RemoveClaimsAsync(user, claims);
        }

        public Task<IdentityResult> RemoveUserClaim(UserModel user, Claim claim)
        {
            return _userManager.RemoveClaimAsync(user, claim);
        }

        public void RemoveUser(UserModel user)
        {
            _database.Users.Remove(user);
        }

        public bool AddWidgetSubscription(int userId, int widgetId)
        {
            var dbSet = _database.Set<UserWidgetModel>();
            var existingSub = dbSet.FirstOrDefault(model => model.UserId == userId && model.WidgetId == widgetId);

            if (existingSub != null)
                return true;

            if (!_widgetRepository.WidgetExists(widgetId))
                return false;

            dbSet.Add(new UserWidgetModel {
                UserId = userId,
                WidgetId = widgetId
            });
            return true;
        }

        public bool RemoveWidgetSubscription(int userId, int widgetId)
        {
            var dbSet = _database.Set<UserWidgetModel>();

            var subscription = dbSet.FirstOrDefault(model => model.UserId == userId && model.WidgetId == widgetId);
            var userParams = GetUser(userId, includeChildren: true, asNoTracking: false)
                ?.WidgetParams
                .Where(model => model.Param.WidgetId == widgetId);

            if (userParams != null)
                _database.RemoveRange(userParams);
            if (subscription == null)
                return false;
            dbSet.Remove(subscription);
            return true;
        }

        public bool AddServiceCredentials(int userId, int serviceId, string jsonTokens)
        {
            var user = _database.Users
                .FirstOrDefault(model => model.Id == userId);

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
            var user = _database.Users
                .FirstOrDefault(model => model.Id == userId);

            var serviceToken = user?.ServiceTokens.FirstOrDefault(model => model.ServiceId == serviceId);

            if (serviceToken != null)
                user!.ServiceTokens.Remove(serviceToken);
        }

        public bool RemoveDevice(int userId, uint deviceId)
        {
            var user = GetUser(userId, asNoTracking: false);
            var device = user?.Devices.FirstOrDefault(model => model.Id == deviceId);

            if (device == null)
                return false;
            user!.Devices.Remove(device);
            return true;
        }
    }
}