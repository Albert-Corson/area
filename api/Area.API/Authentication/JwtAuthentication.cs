using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Area.API.Extensions;
using Area.API.Models.Table.Owned;
using Area.API.Repositories;
using Area.API.Services;
using IpData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wangkanai.Detection.Services;

namespace Area.API.Authentication
{
    public class JwtAuthentication : JwtBearerHandler
    {
        private readonly UserRepository _userRepository;
        private readonly IDetectionService _detection;
        private readonly IpDataClient _ipData;

        public JwtAuthentication(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, UserRepository userRepository, IDetectionService detection, IpDataClient ipData)
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
            _detection = detection;
            _ipData = ipData;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticateResult = await base.HandleAuthenticateAsync();

            if (!authenticateResult.Succeeded)
                return authenticateResult;

            var userId = AuthService.GetUserIdFromPrincipal(authenticateResult.Principal)!;
            var registeredDeviceId = AuthService.GetDeviceIdFromPrincipal(authenticateResult.Principal)!;

            var user = _userRepository.GetUser(userId.Value);
            if (user == null)
                return AuthenticateResult.Fail("The associated user does not exist");

            var currentCountry = await _ipData.GetCountry(Context.Connection.RemoteIpAddress);
            var currentDevice = new UserDeviceModel(_detection, userId.Value, currentCountry);
            if (registeredDeviceId != currentDevice.Id)
                return AuthenticateResult.Fail("Unauthorized use of this token");

            var registeredDevice = user.Devices.FirstOrDefault(model => model.Id == registeredDeviceId);
            if (registeredDevice == null) 
                return AuthenticateResult.Fail("No device associated to this token");

            registeredDevice.LastUsed = DateTime.Now;

            return authenticateResult;
        }
    }
}