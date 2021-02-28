using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Area.AcceptanceTests.Models.Responses
{
    public class ParamModel
    {
        private bool Equals(ParamModel other) => Name == other.Name && Type == other.Type && Value == other.Value && Required == other.Required;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((ParamModel) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, (int) Type, Value, Required);
        }

        public enum ParamType
        {
            [EnumMember(Value = "string")] String,
            [EnumMember(Value = "integer")] Integer,
            [EnumMember(Value = "boolean")] Boolean
        }

        public static bool operator!=(ParamModel self, ParamModel other)
        {
            return !(self == other);
        }

        public static bool operator==(ParamModel self, ParamModel other)
        {
            return self.Name == other.Name
                && self.Required == other.Required
                && self.Type == other.Type
                && self.Value == other.Value;
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