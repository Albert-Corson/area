using Newtonsoft.Json;

namespace Dashboard.API.Models.Request
{
    public class CredentialsModel
    {
        [JsonProperty("username")]
        public string Username { get; set; } = "";

        [JsonProperty("password")]
        public string Password { get; set; } = "";

        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
