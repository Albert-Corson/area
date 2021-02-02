using Newtonsoft.Json;

namespace Area.API.Models.Request
{
    public class RefreshTokenModel
    {
        [JsonProperty("refresh_token", Required = Required.Always)]
        public string RefreshToken { get; set; } = null!;
    }
}
