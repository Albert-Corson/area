using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class StatusModel
    {
        [JsonIgnore]
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore
        };

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
            return JsonConvert.SerializeObject(this, Formatting.None, SerializerSettings);
        }

        public JsonResult ToJsonResult()
        {
            return new JsonResult(this) {
                SerializerSettings = SerializerSettings
            };
        }
    }
}
