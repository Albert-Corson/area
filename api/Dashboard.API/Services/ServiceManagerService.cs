using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Repositories;
using Dashboard.API.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dashboard.API.Services
{
    public class ServiceManagerService
    {
        private readonly DatabaseRepository _database;
        private readonly IDictionary<string, IServiceService> _service;
        private readonly ILogger _logger;

        public ServiceManagerService(DatabaseRepository database, ILoggerFactory loggerFactory,
            ImgurServiceService imgur,
            SpotifyServiceService spotify,
            RedditServiceService reddit)
        {
            _logger = loggerFactory.CreateLogger("Service manager");
            _database = database;
            _service = new Dictionary<string, IServiceService> {
                {imgur.Name, imgur},
                {spotify.Name, spotify},
                {reddit.Name, reddit}
            };
        }

        public Uri? SignInServiceById(HttpContext context, int serviceId)
        {
            var serviceName = _database.Services.FirstOrDefault(model => model.Id == serviceId)?.Name;

            if (serviceName == null)
                throw new NotFoundHttpException();
            if (!_service.TryGetValue(serviceName, out var service))
                return null;

            var userId = AuthService.GetUserIdFromPrincipal(context.User);
            if (userId == null)
                return null;

            var uri = service.SignIn(context, userId.Value);
            _database.SaveChanges();
            return uri;
        }

        public void HandleServiceSignInCallbackById(HttpContext context, int serviceId)
        {
            var serviceName = _database.Services.FirstOrDefault(model => model.Id == serviceId)?.Name;

            if (serviceName == null || !_service.TryGetValue(serviceName, out var service)) {
                _logger.LogError($"Received signin callback with an invalid {{serviceId}} ({serviceId})");
                return;
            }

            var userId = service.GetUserIdFromCallbackContext(context);
            if (userId == null)
                return;

            var user = _database.Users
                .Include(model => model.ServiceTokens)
                .FirstOrDefault(model => model.Id == userId);
            if (user == null)
                return;

            var oldTokens = user.ServiceTokens?.FirstOrDefault(model => model.ServiceId == serviceId);
            if (oldTokens != null)
                _database.Remove(oldTokens);

            service.HandleSignInCallback(context, serviceId, user);
            _database.SaveChanges();
        }
    }
}
