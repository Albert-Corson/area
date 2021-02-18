using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Requests
{
    public class SignInModel
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; } = null!;

        [JsonProperty("password")]
        public string Password { get; set; } = null!;
    }
}