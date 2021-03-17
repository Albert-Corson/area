using Newtonsoft.Json;

namespace Area.API.Models.Request.Password
{
    public class ResetPasswordMailDataModel : object
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; } = null!;
    }
}