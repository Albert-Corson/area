using System;
using System.Collections.Generic;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Repositories;
using Area.API.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Area.API.Services
{
    public class ServiceManagerService
    {
        private readonly ILogger _logger;
        private readonly IDictionary<string, IServiceService> _services;
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
            _services = new Dictionary<string, IServiceService> {
                {imgur.Name, imgur},
                {spotify.Name, spotify}
            };
        }

        public bool TrySignInServiceById(int serviceId, ServiceAuthStateModel state, out string? urlOrError)
        {
            var serviceName = _serviceRepository.GetService(serviceId)?.Name;

            if (serviceName == null) {
                urlOrError = "Service not available";
                return false;
            }

            if (!_services.TryGetValue(serviceName, out var service)) {
                urlOrError = null;
                return true;
            }

            var stateStr = JsonConvert.SerializeObject(state);
            urlOrError = service.SignIn(stateStr)?.ToString() ?? "Unable to authenticate, please try again later.";
            return true;
        }

        public bool HandleServiceSignInCallbackById(HttpContext context, int serviceId,
            ServiceAuthStateModel state)
        {
            var serviceName = _serviceRepository.GetService(serviceId)?.Name;

            if (serviceName == null || !_services.TryGetValue(serviceName, out var service)) {
                _logger.LogError($"Received signin callback with an invalid {{serviceId}} ({serviceId})");
                return false;
            }

            if (!_userRepository.UserExists(state.UserId))
                return false;

            _userRepository.RemoveServiceCredentials(state.UserId, serviceId);

            var jsonTokens = service.HandleSignInCallback(context);
            if (jsonTokens == null)
                return false;
            _userRepository.AddServiceCredentials(state.UserId, serviceId, jsonTokens);
            return true;
        }
    }
}