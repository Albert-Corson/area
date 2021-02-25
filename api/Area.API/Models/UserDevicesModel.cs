    using System.Collections.Generic;
using Area.API.Models.Table.Owned;
using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;

    namespace Area.API.Models
{
    public class UserDevicesModel
    {
        [JsonProperty("current_device", Required = Required.Always)]
        [SwaggerSchema("Id of the device used for this request")]
        public uint CurrentDevice { get; set; }

        [JsonProperty("devices", Required = Required.Always)]
        [SwaggerSchema("List of devices associated to the user")]
        public List<UserDeviceModel> Devices { get; set; } = null!;
    }
}