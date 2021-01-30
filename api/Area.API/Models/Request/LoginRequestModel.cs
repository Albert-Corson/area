using Newtonsoft.Json;

namespace Area.API.Models.Request
{
    public class SignInModel
    {
        [JsonRequired]
        [JsonProperty("identifier")]
        public string? Identifier { get; set; }

        [JsonRequired]
        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
