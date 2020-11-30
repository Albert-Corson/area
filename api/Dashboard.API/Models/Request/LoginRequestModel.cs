using Newtonsoft.Json;

namespace Dashboard.API.Models.Request
{
    public class LoginModel
    {
        [JsonRequired]
        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonRequired]
        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
