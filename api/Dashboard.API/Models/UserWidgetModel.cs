using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.API.Models
{
    public class UserWidgetModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? UserId { get; set; }

        public UserModel? User { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? WidgetId { get; set; }

        public WidgetModel? Widget { get; set; }
    }
}
