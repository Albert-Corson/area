using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Area.API.Extensions;
using Area.API.Repositories;
using Area.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Area.API.Authentication
{
    public class JwtAuthentication : JwtBearerHandler
    {
        private readonly UserRepository _userRepository;
        private readonly AuthService _authService;

        public JwtAuthentication(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, UserRepository userRepository, AuthService authService)
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticateResult = await base.HandleAuthenticateAsync();

            if (!authenticateResult.Succeeded)
                return authenticateResult;

            var user = authenticateResult.Principal.TryGetUserId(out var userId)
                ? _userRepository.GetUser(userId, asNoTracking: false)
                : null;

            if (user == null)
                return AuthenticateResult.Fail("The associated user does not exist");

            if (!await _authService.ValidateDeviceUse(authenticateResult.Principal, user, Context.Connection.RemoteIpAddress.MapToIPv4())) 
                return AuthenticateResult.Fail("No device associated to this token");

            return authenticateResult;
        }
    }
}