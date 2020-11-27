using System.Collections;
using System.ComponentModel.DataAnnotations;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [Route(RoutesConstants.Users.CreateUser)]
        [ValidateModelState]
        public JsonResult UsersPost(
            [FromBody] RegisterModel body
        )
        {
            // TODO: create account from credentials
            return StatusModel.Success();
            return StatusModel.Failed("error message"); // TODO: Username or email taken/Weak password/Invalid email address
        }

        [HttpDelete]
        [Route(RoutesConstants.Users.UserIdDeleteUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult UsersUserIdDelete(
            [FromRoute] [Required] [Range(1, 2147483647)] int? userId
        )
        {
            // TODO: delete the account from the userId
            return StatusModel.Success();
            return StatusModel.Failed("error message"); // TODO: Username or email taken/Weak password/Invalid email address
        }

        [HttpGet]
        [Route(RoutesConstants.Users.UserIdGetUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult UsersUserIdGet(
            [FromRoute] [Required] [Range(1, 2147483647)] int? userId
        )
        {
            // TODO: get user from userId
            // TODO: check the access level of the connected user (from Bearer)

            return new ResponseModel<UserModel> {
                Data = {
                    Id = 42,
                    Username = "username", // TODO
                    Email = null // TODO: for admins only
                }
            };
        }
    }
}
