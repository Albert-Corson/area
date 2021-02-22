using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class AboutDotJsonModel
    {
        [JsonProperty("services", Required = Required.Always)]
        public IEnumerable<ServiceModel> Services { get; set; } = null!;

        public class ServiceModel : Responses.ServiceModel
        {
            [JsonProperty("widgets", Required = Required.Always)]
            public IEnumerable<WidgetModel> Widgets { get; set; } = null!;
        }

        public class WidgetModel
        {
            public void Copy(WidgetModel other)
            {
                Id = other.Id;
                Name = other.Name;
                Description = other.Description;
                RequiresAuth = other.RequiresAuth;
            }
            
            public static bool operator!=(WidgetModel self, WidgetModel other)
            {
                return !(self == other);
            }

            public static bool operator==(WidgetModel self, WidgetModel other)
            {
                return self.RequiresAuth == other.RequiresAuth
                    && self.Description == other.Description
                    && self.Id == other.Id
                    && self.Name == other.Name;
            }
            
            [JsonProperty("id", Required = Required.Always)]
            public int Id { get; set; }

            [JsonProperty("name", Required = Required.Always)]
            public string Name { get; set; } = null!;

            [JsonProperty("description", Required = Required.Always)]
            public string Description { get; set; } = null!;

            [JsonProperty("requires_auth", Required = Required.Always)]
            public bool RequiresAuth { get; set; }
        }
    }
}