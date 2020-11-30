using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.API.Models.Table.ManyToMany
{
    [Table("UsersToServices")]
    public class UserServiceModel
    {
        public int? UserId { get; set; }

        public UserModel? User { get; set; }

        public int? ServiceId { get; set; }

        public ServiceModel? Service { get; set; }
    }
}
