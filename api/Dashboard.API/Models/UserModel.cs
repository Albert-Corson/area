using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Dashboard.API.Models
{
    public class UserModel
    {
        public UserModel()
        { }

        public UserModel(int id = 0, string username = "", string? email = null)
        {
            Id = id;
            Username = username;
            Email = email;
        }

        [ForeignKey("UserId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }

        [JsonIgnore]
        public ICollection<UserWidgetModel>? Widgets { get; set; }

        [JsonIgnore]
        public ICollection<UserServiceModel>? Services { get; set; }
    }
}
