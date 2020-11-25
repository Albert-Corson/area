using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class StatusModel
    {
        public StatusModel()
        { }

        public StatusModel(bool successful, string? error = null)
        {
            Successful = successful;
            Error = error;
        }

        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("successful")]
        public bool Successful { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
