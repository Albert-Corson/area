using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasServiceTokens")]
    public class UserServiceTokensModel
    {
        public string? Scheme { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public int? ServiceId { get; set; }

        public ServiceModel? Service { get; set; }
    }
}
