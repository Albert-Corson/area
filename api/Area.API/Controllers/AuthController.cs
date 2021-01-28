using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Request;
using Area.API.Repositories;
using Area.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Area.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        private readonly UserRepository _userRepository;

        public AuthController(AuthService service, UserRepository userRepository)
        {
            _service = service;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.SignIn)]
        [ValidateModelState]
        public JsonResult SignIn(
            [FromBody] SignInModel body
        )
        {
            var user = _userRepository.GetUser(body.Username!, body.Password!);
            if (user?.Id == null)
                throw new UnauthorizedHttpException();

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = _service.GenerateRefreshToken(user.Id.Value),
                    AccessToken = _service.GenerateAccessToken(user.Id.Value)
                }
            };
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.RefreshAccessToken)]
        [ValidateModelState]
        public JsonResult RefreshAccessToken(
            [FromBody] RefreshTokenModel body
        )
        {
            var userId = _service.GetUserIdFromRefreshToken(body.RefreshToken!);

            if (userId == null)
                throw new UnauthorizedHttpException();

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = _service.GenerateRefreshToken(userId.Value),
                    AccessToken = _service.GenerateAccessToken(userId.Value)
                }
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Auth.RevokeUserTokens)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult RevokeUserTokens()
        {
            // TODO: (optional) revoke the credentials
            return StatusModel.Failed("Not implemented");
        }
    }
}
