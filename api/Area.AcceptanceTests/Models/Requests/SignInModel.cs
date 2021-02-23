using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Requests
{
    public class SignInModel
    {
        public SignInModel()
        { }

        public SignInModel(RegisterModel registerForm)
        {
            Identifier = registerForm.Email;
            Password = registerForm.Password;
        }

        [JsonProperty("identifier")]
        public string Identifier { get; set; } = null!;

        [JsonProperty("password")]
        public string Password { get; set; } = null!;
    }
}