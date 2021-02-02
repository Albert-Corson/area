using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Area.API.Models.Table.ManyToMany;
using Area.API.Models.Table.Owned;
using Newtonsoft.Json;

namespace Area.API.Models.Table
{
    public class UserModel
    {
        [ForeignKey("UserId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; } = null!;

        [JsonProperty("email")]
        public string Email { get; set; } = null!;

        [JsonIgnore]
        public string Password { get; set; } = null!;

        [JsonIgnore]
        public ICollection<UserWidgetModel> Widgets { get; set; } = null!;

        [JsonIgnore]
        public ICollection<UserServiceTokensModel> ServiceTokens { get; set; } = null!;

        [JsonIgnore]
        public ICollection<UserWidgetParamModel> WidgetParams { get; set; } = null!;
    }
}