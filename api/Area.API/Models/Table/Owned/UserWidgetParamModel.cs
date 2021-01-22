using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasWidgetParams")]
    public class UserWidgetParamModel : WidgetParamModel
    {
        public int? WidgetId { get; set; }

        public WidgetModel? Widget { get; set; }

        [NotMapped]
        [JsonIgnore]
        public override bool? Required => true;
    }
}
