using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Request
{
    public class RefreshTokenModel
    {
        [JsonProperty("refresh_token", Required = Required.Always)]
        [SwaggerSchema("Token to get a new pair of tokens")]
        public string RefreshToken { get; set; } = null!;
    }
}