using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class StatusModel
    {
        [JsonProperty("error", Required = Required.DisallowNull)]
        public string? Error { get; set; }

        [JsonProperty("successful", Required = Required.Always)]
        public bool Successful { get; set; } = true;
    }
}