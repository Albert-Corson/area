using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Dashboard.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.RefreshAccessToken)]
        [ValidateModelState]
        public JsonResult AuthRefreshPost(
            [FromBody] RefreshTokenModel body
        )
        {
            var userId = _service.GetUserIdFromRefreshToken(body.RefreshToken!);

            if (userId == null)
                throw new UnauthorizedHttpException("Invalid refresh token");

            return new ResponseModel<UserTokenModel> {
                Data = {
                    RefreshToken = _service.GenerateRefreshToken(userId.Value),
                    AccessToken = _service.GenerateAccessToken(userId.Value)
                }
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Auth.RevokeAccountTokens)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult AuthRevokeDelete()
        {
            // TODO: revoke the credentials, check with Adrien how this is supposed to be done
            return StatusModel.Failed("error message"); // TODO: if failed
            return StatusModel.Success();
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.Login)]
        [ValidateModelState]
        public JsonResult AuthTokenPost(
            [FromBody] LoginModel body
        )
        {
            return new ResponseModel<UserTokenModel> {
                Data = {
                    RefreshToken = _service.GenerateRefreshToken(0), // TODO: put real userId
                    AccessToken = _service.GenerateAccessToken(0), // TODO: put real userId
                }
            };
            return StatusModel.Failed("error message"); // TODO: if failed
        }
    }
}
