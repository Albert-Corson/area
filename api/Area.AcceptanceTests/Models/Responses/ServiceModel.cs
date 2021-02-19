using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class ServiceModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; } = null!;

        [JsonProperty("widgets", Required = Required.DisallowNull)]
        public IEnumerable<WidgetModel> Widgets { get; set; } = null!;
    }
}