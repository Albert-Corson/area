using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.API.Models.Response;
using Newtonsoft.Json;

namespace Dashboard.API.Models
{
    public class WidgetModel
    {
        public WidgetModel()
        { }

        public WidgetModel(int id = 0, string name = "", ServiceModel? service = null)
        {
            Id = id;
            Name = name;
            if (service != null)
                Service = service;
        }

        [ForeignKey("WidgetId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonIgnore]
        public int? ServiceId { get; set; }

        [JsonProperty("service")]
        public ServiceModel? Service { get; set; }

        [JsonIgnore]
        public ICollection<UserWidgetModel>? Users { get; set; }
    }
}
