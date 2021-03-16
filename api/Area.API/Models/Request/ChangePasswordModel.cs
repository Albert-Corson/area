using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Request
{
    public class ChangePasswordModel
    {
        [JsonProperty("old_password", Required = Required.Always)]
        [SwaggerSchema("The old password.")]
        public string Old { get; set; } = null!;

        [JsonProperty("new_password", Required = Required.Always)]
        [SwaggerSchema("The new password.")]
        public string New { get; set; } = null!;

        [JsonProperty("reset_devices", Required = Required.DisallowNull)]
        [SwaggerSchema("A boolean to reset the connection of all the other devices connected to this account.")]
        public bool ResetDevices { get; set; } = false;
    }
}