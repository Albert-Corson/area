using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.API.Models
{
    public class UserServiceModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? UserId { get; set; }

        public UserModel? User { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? ServiceId { get; set; }

        public ServiceModel? Service { get; set; }
    }
}
