using System.ComponentModel.DataAnnotations;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Table;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [Route(RoutesConstants.Users.SignUp)]
        [ValidateModelState]
        public JsonResult SignUp(
            [FromBody] RegisterModel body
        )
        {
            // TODO: create account from credentials
            return StatusModel.Success();
            return StatusModel.Failed("error message"); // TODO: Username or email taken/Weak password/Invalid email address
        }

        [HttpGet]
        [Route(RoutesConstants.Users.GetUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult GetUser(
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

        [HttpGet]
        [Route(RoutesConstants.Users.GetMyUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetMyUser()
        {
            // TODO: get user from bearer

            return new ResponseModel<UserModel> {
                Data = {
                    Id = 42,
                    Username = "username", // TODO
                    Email = null
                }
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Users.DeleteUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult DeleteUser(
            [FromRoute] [Required] [Range(1, 2147483647)] int? userId
        )
        {
            // TODO: delete the account from the userId
            return StatusModel.Success();
            return StatusModel.Failed("error message"); // TODO: Username or email taken/Weak password/Invalid email address
        }
    }
}
