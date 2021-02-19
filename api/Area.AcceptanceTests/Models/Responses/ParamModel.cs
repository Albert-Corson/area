using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Area.AcceptanceTests.Models.Responses
{
    public class ParamModel
    {
        public enum ParamType
        {
            [EnumMember(Value = "string")] String,
            [EnumMember(Value = "integer")] Integer,
            [EnumMember(Value = "boolean")] Boolean
        }

        [JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        public string Name { get; set; } = null!;

        [JsonProperty("type", Required = Newtonsoft.Json.Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParamType Type { get; set; }

        [JsonProperty("value", Required = Newtonsoft.Json.Required.DisallowNull)]
        public string? Value { get; set; }

        [JsonProperty("required", Required = Newtonsoft.Json.Required.Always)]
        public bool Required { get; set; }
    }
}