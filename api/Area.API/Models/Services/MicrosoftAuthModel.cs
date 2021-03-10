using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Area.API.Models.Services
{
    public class MicrosoftAuthModel : IAuthenticationProvider
    {
        public MicrosoftAuthModel()
        { }

        public MicrosoftAuthModel(AuthenticationResult authenticationResult)
        {
            Header = authenticationResult.CreateAuthorizationHeader();
            ExpiresOn = authenticationResult.ExpiresOn;
        }

        public DateTimeOffset ExpiresOn { get; set; }

        public string Header { get; set; } = null!;

        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", Header);
            return Task.CompletedTask;
        }
    }
}