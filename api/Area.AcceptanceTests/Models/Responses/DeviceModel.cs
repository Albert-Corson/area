using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swan;
using Wangkanai.Detection.Models;

namespace Area.AcceptanceTests.Models.Responses
{
    public sealed class DeviceModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public uint Id { get; set; }

        [JsonProperty("last_used", Required = Required.Always)]
        public long LastUsed { get; set; } = DateTime.UtcNow.ToUnixEpochDate();

        [JsonProperty("country", Required = Required.Always)]
        public string Country { get; set; } = "Unknown";

        [JsonProperty("device", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Device Device { get; set; }

        [JsonProperty("browser", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Browser Browser { get; set; }

        [JsonProperty("browser_version", Required = Required.Always)]
        public string BrowserVersion { get; set; } = null!;

        [JsonProperty("os", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform Os { get; set; }

        [JsonProperty("os_version", Required = Required.Always)]
        public string OsVersion { get; set; } = null!;

        [JsonProperty("architecture", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Processor Architecture { get; set; }
    }
}