using Newtonsoft.Json;

namespace Area.API.Models.Request
{
    public class RefreshTokenModel
    {
        [JsonRequired]
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
