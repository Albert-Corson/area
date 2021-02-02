using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasWidgetParams")]
    public class UserWidgetParamModel : WidgetParamModel
    {
        public int WidgetId { get; set; }

        public WidgetModel Widget { get; set; } = null!;

        [NotMapped]
        [JsonProperty("required")]
        public override bool Required => false;
    }
}