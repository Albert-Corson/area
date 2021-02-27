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
    public class UserRepository : ARepository
    {
        private readonly WidgetRepository _widgetRepository;
        private readonly UserManager<UserModel> _userManager;

        public UserRepository(AreaDbContext database, WidgetRepository widgetRepository, UserManager<UserModel> userManager) : base(database)
        {
            _widgetRepository = widgetRepository;
            _userManager = userManager;
        }

        public bool UserExists(int? userId = null, string? username = null, string? email = null, UserModel.UserType? type = null)
        {
            return GetUser(userId, username, email, type: type) != null;
        }

        public UserModel? GetUser(int? userId = null, string? username = null, string? email = null,
            string? passwd = null, UserModel.UserType? type = null, bool includeChildren = false, bool asNoTracking = true)
        {
            var queryable = asNoTracking ? Database.Users.AsNoTracking() : Database.Users.AsQueryable();
            queryable = includeChildren
                ? queryable
                    .Include(model => model.WidgetParams).ThenInclude(model => model.Param)
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

        public Task<IdentityResult> AddUser(UserModel user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> AddUser(UserModel user)
        {
            return _userManager.CreateAsync(user);
        }

        public Task<IdentityResult> AddUserClaim(UserModel user, Claim claim)
        {
            return _userManager.AddClaimAsync(user, claim);
        }

        public Task<IList<Claim>> GetUserClaims(UserModel user)
        {
            return _userManager.GetClaimsAsync(user);
        }
        
        public IdentityUserClaim<int>? GetUserClaim(int userId, string claimType)
        {
            return Database.UserClaims.FirstOrDefault(claim => claim.UserId == userId && claim.ClaimType == claimType);
        }

        public Task<IdentityResult> RemoveUserClaims(UserModel user, IEnumerable<Claim> claims)
        {
            return _userManager.RemoveClaimsAsync(user, claims);
        }

        public Task<IdentityResult> RemoveUserClaim(UserModel user, Claim claim)
        {
            return _userManager.RemoveClaimAsync(user, claim);
        }

        public void RemoveUser(UserModel user)
        {
            Database.Users.Remove(user);
        }

        public bool AddWidgetSubscription(int userId, int widgetId)
        {
            var dbSet = Database.Set<UserWidgetModel>();
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
            var dbSet = Database.Set<UserWidgetModel>();

            var subscription = dbSet.FirstOrDefault(model => model.UserId == userId && model.WidgetId == widgetId);
            var userParams = GetUser(userId, includeChildren: true, asNoTracking: false)
                ?.WidgetParams
                .Where(model => model.Param.WidgetId == widgetId);

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
            var user = Database.Users
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