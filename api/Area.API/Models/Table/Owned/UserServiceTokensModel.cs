using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasServiceTokens")]
    public class UserServiceTokensModel
    {
        public string Json { get; set; } = null!;

        public int ServiceId { get; set; }

        public ServiceModel Service { get; set; } = null!;
    }
}