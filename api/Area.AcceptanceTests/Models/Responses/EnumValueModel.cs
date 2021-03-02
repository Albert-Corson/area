using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class EnumValueModel
    {
        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; } = null!;

        [JsonProperty("display_name", Required = Required.Always)]
        public string DisplayName { get; set; } = null!;
    }
}