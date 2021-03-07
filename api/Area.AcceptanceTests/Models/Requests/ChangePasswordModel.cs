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

        [JsonProperty("old_password")]
        public string Old { get; set; } = null!;

        [JsonProperty("new_password")]
        public string New { get; set; } = null!;

        [JsonProperty("reset_devices")]
        public bool ResetDevices { get; set; } = false;
    }
}