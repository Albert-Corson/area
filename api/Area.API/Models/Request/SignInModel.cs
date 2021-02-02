using Newtonsoft.Json;

namespace Area.API.Models.Request
{
    public class SignInModel
    {
        [JsonProperty("identifier", Required = Required.Always)]
        public string Identifier { get; set; } = null!;

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; } = null!;
    }
}