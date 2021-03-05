using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Area.API.Constants;
using Area.API.Models.Services;
using Area.API.Models.Table.Owned;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace Area.API.Services.Services
{
    public class MicrosoftService : IService
    {
        private readonly AuthorizationCodeProvider _microsoftProvider;
        private readonly IEnumerable<string> _scopes = new[] {"user.read"};

        public MicrosoftService(IConfiguration configuration)
        {
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(configuration[AuthConstants.Microsoft.ClientId])
                .WithClientSecret(configuration[AuthConstants.Microsoft.ClientSecret])
                .WithRedirectUri(configuration[AuthConstants.Microsoft.ServiceRedirectUri])
                .Build();

            _microsoftProvider = new AuthorizationCodeProvider(confidentialClientApplication, _scopes);
        }

        public int Id { get; } = 7;

        public GraphServiceClient? Client { get; private set; }

        public async Task<Uri> GetSignInUrlAsync(string state)
        {
            return await _microsoftProvider.ClientApplication.GetAuthorizationRequestUrl(_scopes)
                .WithExtraQueryParameters(new Dictionary<string, string> {
                    {"state", state}
                })
                .ExecuteAsync();
        }

        public async Task<string?> HandleSignInCallbackAsync(string code)
        {
            try {
                var authenticationResult = await _microsoftProvider.GetAccessTokenByAuthorizationCode(code);

                return JsonConvert.SerializeObject(new MicrosoftAuthModel(authenticationResult));
            } catch {
                return null;
            }
        }

        public bool SignIn(UserServiceTokensModel tokens)
        {
            try {
                var auth = JsonConvert.DeserializeObject<MicrosoftAuthModel>(tokens.Json);

                if (auth.ExpiresOn <= DateTimeOffset.Now)
                    return false;
                Client = new GraphServiceClient(auth);
                return true;
            } catch {
                return false;
            }
        }
    }
}