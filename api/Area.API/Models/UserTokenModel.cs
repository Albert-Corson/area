using Area.API.Constants;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    public class UserTokenModel
    {
        [JsonProperty("access_token", Required = Required.Always)]
        [SwaggerSchema("Bearer token to access the account")]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("expires_in", Required = Required.Always)]
        [SwaggerSchema("Lifetime in seconds of the access token")]
        public long ExpiresIn { get; set; } =  JwtConstants.AccessTokenLifespanSeconds;

        [JsonProperty("refresh_token", Required = Required.Always)]
        [SwaggerSchema("Token to get a new pair of tokens")]
        public string RefreshToken { get; set; } = null!;
    }
}