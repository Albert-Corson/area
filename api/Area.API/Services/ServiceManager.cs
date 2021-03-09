using System.Collections.Generic;
using System.Threading.Tasks;
using Area.API.Repositories;
using Area.API.Services.Services;
using Microsoft.Extensions.Logging;

namespace Area.API.Services
{
    public class ServiceManager
    {
        private readonly ILogger _logger;
        private readonly IDictionary<int, IService> _services;
        private readonly UserRepository _userRepository;

        public ServiceManager(ILoggerFactory loggerFactory,
            UserRepository userRepository,
            ImgurService imgur,
            SpotifyService spotify,
            MicrosoftService microsoft)
        {
            _userRepository = userRepository;
            _logger = loggerFactory.CreateLogger("Service manager");
            _services = new Dictionary<int, IService> {
                {imgur.Id, imgur},
                {spotify.Id, spotify},
                {microsoft.Id, microsoft}
            };
        }

        public bool TryGetServiceById(int serviceId, out IService? service)
        {
            if (_services.TryGetValue(serviceId, out service))
                return true;
            service = null;
            return false;
        }

        public async Task<bool> HandleServiceSignInCallbackById(int serviceId, int userId, string code)
        {
            if (!_services.TryGetValue(serviceId, out var service)) {
                _logger.LogError($"Received signin callback with an invalid {{serviceId}} ({serviceId})");
                return false;
            }

            if (!_userRepository.UserExists(userId))
                return false;

            _userRepository.RemoveServiceCredentials(userId, serviceId);

            var jsonTokens = await service.HandleSignInCallbackAsync(code);
            if (jsonTokens == null)
                return false;
            _userRepository.AddServiceCredentials(userId, serviceId, jsonTokens);
            return true;
        }
    }
}