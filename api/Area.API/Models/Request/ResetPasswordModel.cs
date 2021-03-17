using Newtonsoft.Json;

namespace Area.API.Models.Request
{
    public class ResetPasswordModel
    {
        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; } = null!;

        [JsonProperty("token", Required = Required.Always)]
        public string Token { get; set; } = null!;
    }
}