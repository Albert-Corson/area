using Newtonsoft.Json;

namespace Dashboard.API.Models.Request
{
    public class RegisterModel
    {
        [JsonRequired]
        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonRequired]
        [JsonProperty("password")]
        public string? Password { get; set; }

        [JsonRequired]
        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
