using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Requests
{
    public class RefreshTokenModel
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = null!;
    }
}