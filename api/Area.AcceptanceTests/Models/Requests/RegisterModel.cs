using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Requests
{
    public class RegisterModel
    {
        public RegisterModel()
        { }

        public RegisterModel(RegisterModel other)
        {
            Email = other.Email;
            Username = other.Username;
            Password = other.Password;
        }

        [JsonProperty("username")]
        public string Username { get; set; } = null!;

        [JsonProperty("password")]
        public string Password { get; set; } = null!;

        [JsonProperty("email")]
        public string Email { get; set; } = null!;
    }
}