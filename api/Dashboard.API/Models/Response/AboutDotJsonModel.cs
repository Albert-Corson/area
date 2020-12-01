using System.Collections;
using System.Collections.Generic;
using Dashboard.API.Models.Table;
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
