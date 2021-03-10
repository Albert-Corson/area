using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models
{
    public class ServiceAuthStateModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public int UserId;

        [JsonProperty("redirect_url", Required = Required.Always)]
        public string RedirectUrl { get; set; } = null!;

        [JsonProperty("state")]
        public string? State { get; set; }
    }

}