using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class DevicesModel
    {
        [JsonProperty("current_device", Required = Required.Always)]
        public uint CurrentDevice { get; set; }

        [JsonProperty("devices", Required = Required.Always)]
        public List<DeviceModel> Devices { get; set; } = null!;
    }
}