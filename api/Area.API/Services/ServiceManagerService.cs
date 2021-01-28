using System;
using System.Collections.Generic;
using Area.API.Exceptions.Http;
using Area.API.Repositories;
using Area.API.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Area.API.Services
{
    public class ServiceManagerService
    {
        private readonly IDictionary<string, IServiceService> _service;
        private readonly ILogger _logger;
        private readonly ServiceRepository _serviceRepository;
        private readonly UserRepository _userRepository;

        public ServiceManagerService(ILoggerFactory loggerFactory,
            ServiceRepository serviceRepository,
            UserRepository userRepository,
            ImgurServiceService imgur,
            SpotifyServiceService spotify)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _logger = loggerFactory.CreateLogger("Service manager");
            _service = new Dictionary<string, IServiceService> {
                {imgur.Name, imgur},
                {spotify.Name, spotify},
            };
        }

        public Uri? SignInServiceById(HttpContext context, int serviceId)
        {
            var serviceName = _serviceRepository.GetService(serviceId)?.Name;

            if (serviceName == null)
                throw new NotFoundHttpException();
            if (!_service.TryGetValue(serviceName, out var service))
                return null;

            var userId = AuthService.GetUserIdFromPrincipal(context.User);
            if (userId == null)
                return null;

            var uri = service.SignIn(context, userId.Value);
            return uri;
        }

        public bool HandleServiceSignInCallbackById(HttpContext context, int serviceId)
        {
            var serviceName = _serviceRepository.GetService(serviceId)?.Name;

            if (serviceName == null || !_service.TryGetValue(serviceName, out var service)) {
                _logger.LogError($"Received signin callback with an invalid {{serviceId}} ({serviceId})");
                return false;
            }

            var userId = service.GetUserIdFromCallbackContext(context);
            if (userId == null || !_userRepository.UserExists(userId))
                return false;

            _userRepository.RemoveServiceCredentials(userId.Value, serviceId);

            var jsonTokens = service.HandleSignInCallback(context);
            if (jsonTokens == null)
                return false;
            _userRepository.AddServiceCredentials(userId.Value, serviceId, jsonTokens);
            return true;
        }
    }
}
