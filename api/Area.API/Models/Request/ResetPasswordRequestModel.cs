using Newtonsoft.Json;

namespace Area.API.Models.Request
{
    public class ResetPasswordRequestModel
    {
        [JsonProperty("redirect_url", Required = Required.Always)]
        public string RedirectUrl { get; set; } = null!;

        [JsonProperty("identifier", Required = Required.Always)]
        public string Identifier { get; set; } = null!;
    }
}