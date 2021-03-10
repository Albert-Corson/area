using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    public class AuthenticationRedirectModel
    {
        [JsonProperty("redirect_url", Required = Required.DisallowNull)]
        [SwaggerSchema("URL to redirect the user to, to complete authentication")]
        public string? RedirectUrl { get; set; }

        [JsonProperty("requires_redirect", Required = Required.Always)]
        [SwaggerSchema("Indicates if the client should redirect the user to the given URL to complete authentication")]
        public bool RequiresRedirect => RedirectUrl != null;
    }
}