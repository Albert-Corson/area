using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("successful")]
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
