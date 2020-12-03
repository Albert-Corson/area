using System.Collections.Generic;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class AboutDotJsonModel
    {
        public class ClientModel
        {
            [JsonProperty("host")]
            public string? Host { get; set; }
        }

        public class WidgetModel
        {
            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("description")]
            public string? Description { get; set; }

            [JsonProperty("params")]
            public IEnumerable<WidgetParamModel>? DefaultParams;
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
