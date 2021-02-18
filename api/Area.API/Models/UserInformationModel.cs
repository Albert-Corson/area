using Area.API.Models.Table;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    public class UserInformationModel
    {
        public UserInformationModel(UserModel user)
        {
            Id = user.Id;
            Email = user.Email;
            UserName = user.UserName;
        }

        [JsonProperty("id", Required = Required.Always)]
        [SwaggerSchema("The user's ID")]
        public int Id { get; set; }

        [JsonProperty("username", Required = Required.Always)]
        [SwaggerSchema("The user's username")]
        public string UserName { get; set; }

        [JsonProperty("email", Required = Required.Always)]
        [SwaggerSchema("The user's email")]
        public string Email { get; set; }
    }
}