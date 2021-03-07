using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Request
{
    public class ChangePasswordModel
    {
        [JsonProperty("old_password", Required = Required.Always)]
        public string Old { get; set; } = null!;

        [JsonProperty("new_password", Required = Required.Always)]
        public string New { get; set; } = null!;

        [JsonProperty("reset_devices", Required = Required.DisallowNull)]
        public bool ResetDevices { get; set; } = false;
    }
}