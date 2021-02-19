using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class WidgetModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; } = null!;

        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; } = null!;

        [JsonProperty("requires_auth", Required = Required.Always)]
        public bool RequiresAuth { get; set; }

        [JsonProperty("frequency", Required = Required.Always)]
        public int Frequency { get; set; }

        [JsonProperty("service", Required = Required.DisallowNull)]
        public ServiceModel Service { get; set; } = null!;

        [JsonProperty("params", Required = Required.DisallowNull)]
        public ICollection<ParamModel> Params { get; set; } = null!;
    }
}