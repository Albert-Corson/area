using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class UserInformationModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; set; } = null!;

        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; } = null!;
    }
}