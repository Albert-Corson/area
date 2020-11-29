using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Table.Owned
{
    [Owned]
    public class WidgetParamModel
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonIgnore]
        public string? Value { get; set; }
    }
}
