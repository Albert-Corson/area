using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.API.Models
{
    public class AboutDotJsonModel
    {
        public class ClientModel
        {
            [JsonProperty("host")]
            public string Host { get; set; } = null!;
        }

        public class WidgetParamModel
        {
            [JsonProperty("name")]
            public string Name { get; set; } = null!;

            [JsonProperty("type")]
            public string Type { get; set; } = null!;
        }

        public class WidgetModel
        {
            [JsonProperty("name")]
            public string Name { get; set; } = null!;

            [JsonProperty("description")]
            public string Description { get; set; } = null!;

            [JsonProperty("params")]
            public IEnumerable<WidgetParamModel> Params = null!;
        }

        public class ServiceModel
        {
            [JsonProperty("name")]
            public string Name { get; set; } = null!;

            [JsonProperty("widgets")]
            public IEnumerable<WidgetModel> Widgets { get; set; } = null!;
        }

        public class ServerModel
        {
            [JsonProperty("current_time")]
            public long CurrentTime { get; set; }

            [JsonProperty("services")]
            public IEnumerable<ServiceModel> Services { get; set; } = null!;
        }

        [JsonProperty("client")]
        public ClientModel Client { get; set; } = null!;

        [JsonProperty("server")]
        public ServerModel Server { get; set; } = null!;
    }
}