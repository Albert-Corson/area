using System.ComponentModel.DataAnnotations.Schema;

namespace Area.API.Models.Table.ManyToMany
{
    [Table("UsersToWidgets")]
    public class UserWidgetModel
    {
        public int UserId { get; set; }

        public UserModel User { get; set; } = null!;

        public int WidgetId { get; set; }

        public WidgetModel Widget { get; set; } = null!;
    }
}