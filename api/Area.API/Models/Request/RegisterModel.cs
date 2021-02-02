using Newtonsoft.Json;

namespace Area.API.Models.Request
{
    public class RegisterModel
    {
        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; set; } = null!;

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; } = null!;

        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; } = null!;
    }
}