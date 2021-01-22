using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Area.API.Models
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
        public bool? Successful { get; set; } = true;

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

        public static StatusModel Failed(string? error = null)
        {
            return new StatusModel(false, error);
        }

        public static StatusModel Success()
        {
            return new StatusModel(true);
        }

        public static implicit operator JsonResult(StatusModel self)
        {
            return self.ToJsonResult();
        }
    }
}
