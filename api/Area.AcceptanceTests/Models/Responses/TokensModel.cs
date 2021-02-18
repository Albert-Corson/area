using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class TokensModel
    {
        [JsonProperty("access_token", Required = Required.Always)]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("expires_in", Required = Required.Always)]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token", Required = Required.Always)]
        public string RefreshToken { get; set; } = null!;
    }
}