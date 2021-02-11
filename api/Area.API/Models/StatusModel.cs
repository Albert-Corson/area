using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
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

        [JsonProperty("error", Required = Required.DisallowNull)]
        [SwaggerSchema("Message describing the error if any")]
        public string? Error { get; set; }

        [JsonProperty("successful", Required = Required.Always)]
        [SwaggerSchema("State of the request (failed or successful)")]
        public bool Successful { get; set; } = true;

        public static StatusModel Failed(string? error = null)
        {
            return new StatusModel(false, error);
        }

        public static StatusModel Success()
        {
            return new StatusModel(true);
        }
    }
}