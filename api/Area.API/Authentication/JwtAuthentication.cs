using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
        private readonly DatabaseRepository _database;

        public JwtAuthentication(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, DatabaseRepository database)
            : base(options, logger, encoder, clock)
        {
            _database = database;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticateResult = await base.HandleAuthenticateAsync();

            if (!authenticateResult.Succeeded) {
                return authenticateResult;
            }

            var userId = AuthService.GetUserIdFromPrincipal(authenticateResult.Principal);

            if (userId == null || _database.Users.FirstOrDefault(model => model.Id == userId) == null)
                return AuthenticateResult.Fail("This user does not exist");

            return authenticateResult;
        }
    }
}
