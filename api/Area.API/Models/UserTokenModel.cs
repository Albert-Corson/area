using Area.API.Constants;
using Newtonsoft.Json;

namespace Area.API.Models
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
