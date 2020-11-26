using System.Collections;
using System.ComponentModel.DataAnnotations;
using Dashboard.API.Attributes;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dashboard.API.Controllers
{
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("/users")]
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
        [Route("/users/{userId}")]
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
        [Route("/users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult UserGet()
        {
            var users = new ArrayList();
            // TODO: check the access level of the connected user (from Bearer)
            // TODO: get all users (users.Add(...))

            return new ResponseModel<ArrayList> {
                Data = users
            };
        }

        [HttpGet]
        [Route("/users/{userId}")]
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
