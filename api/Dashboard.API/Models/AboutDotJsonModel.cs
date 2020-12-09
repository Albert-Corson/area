using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dashboard.API.Models
{
    public class AboutDotJsonModel
    {
        public class ClientModel
        {
            [JsonProperty("host")]
            public string? Host { get; set; }
        }

        public class WidgetParamModel
        {
            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("type")]
            public string? Type { get; set; }
        }

        public class WidgetModel
        {
            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("description")]
            public string? Description { get; set; }

            [JsonProperty("params")]
            public IEnumerable<WidgetParamModel>? Params;
        }

        public class ServiceModel
        {
            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("widgets")]
            public IEnumerable<WidgetModel>? Widgets { get; set; }
        }

        public class ServerModel
        {
            [JsonProperty("current_time")]
            public long? CurrentTime { get; set; }

            [JsonProperty("services")]
            public IEnumerable<ServiceModel>? Services { get; set; }
        }

        [JsonProperty("client")]
        public ClientModel? Client { get; set; }

        [JsonProperty("server")]
        public ServerModel? Server { get; set; }
    }
}
