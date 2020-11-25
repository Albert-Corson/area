using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
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

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; } = "";

        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
