using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Area.API.Models.Table.ManyToMany;
using Area.API.Models.Table.Owned;
using Newtonsoft.Json;

namespace Area.API.Models.Table
{
    [Table("Widgets")]
    public class WidgetModel
    {
        [ForeignKey("WidgetId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("requires_auth")]
        public bool? RequiresAuth { get; set; }

        [JsonIgnore]
        public int? ServiceId { get; set; }

        [JsonProperty("service")]
        public ServiceModel? Service { get; set; }

        [JsonIgnore]
        public ICollection<UserWidgetModel>? Users { get; set; }

        [JsonProperty("params")]
        public ICollection<WidgetParamModel>? Params;
    }
}
