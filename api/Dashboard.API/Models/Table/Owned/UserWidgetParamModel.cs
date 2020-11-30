using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasWidgetParams")]
    public class UserWidgetParamModel : WidgetParamModel
    {
        public int? WidgetId { get; set; }

        public WidgetModel? Widget { get; set; }
    }
}
