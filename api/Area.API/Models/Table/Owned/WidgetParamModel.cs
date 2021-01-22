using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("WidgetHasParams")]
    public class WidgetParamModel
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("required")]
        public virtual bool? Required { get; set; }
    }
}
