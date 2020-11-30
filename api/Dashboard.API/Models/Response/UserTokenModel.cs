using Dashboard.API.Constants;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class UserTokenModel
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long? ExpiresIn { get; set; } = JwtConstants.AccessTokenLifespan;

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
