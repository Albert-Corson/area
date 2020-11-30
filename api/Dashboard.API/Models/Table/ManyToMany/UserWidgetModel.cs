using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.API.Models.Table.ManyToMany
{
    [Table("UsersToWidgets")]
    public class UserWidgetModel
    {
        public int? UserId { get; set; }

        public UserModel? User { get; set; }

        public int? WidgetId { get; set; }

        public WidgetModel? Widget { get; set; }
    }
}
