using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class AuthenticationRedirectModel
    {
        [JsonProperty("redirect_url", Required = Required.DisallowNull)]
        public string? RedirectUrl { get; set; }

        [JsonProperty("requires_redirect", Required = Required.Always)]
        public bool RequiresRedirect => RedirectUrl != null;
    }
}