using System.Collections.Generic;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    public class AboutDotJsonModel
    {

        [JsonProperty("services", Required = Required.Always)]
        [SwaggerSchema("List of all available services")]
        public IEnumerable<ServiceModel> Services { get; set; } = null!;

        public class WidgetModel
        {
            [JsonProperty("id", Required = Required.Always)]
            [SwaggerSchema("Widget's ID")]
            public int Id { get; set; }

            [JsonProperty("name", Required = Required.Always)]
            [SwaggerSchema("Widget's name")]
            public string Name { get; set; } = null!;

            [JsonProperty("description", Required = Required.Always)]
            [SwaggerSchema("Widget's description")]
            public string Description { get; set; } = null!;

            [JsonProperty("requires_auth", Required = Required.Always)]
            [SwaggerSchema("Indicates if authentication to the parent service is required in order to use the widget")]
            public bool RequiresAuth { get; set; }
        }

        public class ServiceModel
        {
            [JsonProperty("id", Required = Required.Always)]
            [SwaggerSchema("Service's ID")]
            public int Id { get; set; }

            [JsonProperty("name", Required = Required.Always)]
            [SwaggerSchema("Service's name")]
            public string Name { get; set; } = null!;

            [JsonProperty("widgets", Required = Required.Always)]
            [SwaggerSchema("List of widgets in the service")]
            public IEnumerable<WidgetModel> Widgets { get; set; } = null!;
        }
    }
}