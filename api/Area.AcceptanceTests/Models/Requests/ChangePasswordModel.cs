using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Requests
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel()
        { }

        public ChangePasswordModel(ChangePasswordModel rhs)
        {
            Old = rhs.Old;
            New = rhs.New;
            ResetDevices = rhs.ResetDevices;
        }

        [JsonProperty("old_password", Required = Required.Always)]
        public string Old { get; set; } = null!;

        [JsonProperty("new_password", Required = Required.Always)]
        public string New { get; set; } = null!;

        [JsonProperty("reset_devices", Required = Required.DisallowNull)]
        public bool ResetDevices { get; set; } = false;
    }
}