using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Area.API.Models.Table
{
    public class ServiceModel
    {
        [ForeignKey("ServiceId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public ICollection<WidgetModel> Widgets { get; set; } = null!;
    }
}