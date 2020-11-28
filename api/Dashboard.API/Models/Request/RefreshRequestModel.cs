using Newtonsoft.Json;

namespace Dashboard.API.Models.Request
{
    public class RefreshTokenModel
    {
        [JsonRequired]
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
