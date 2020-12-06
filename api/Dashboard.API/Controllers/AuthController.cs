using System;
using System.Linq;
using Dashboard.API.Attributes;
using Dashboard.API.Common;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Dashboard.API.Repositories;
using Dashboard.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dashboard.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        private readonly IConfiguration _configuration;
        private readonly DatabaseRepository _database;

        public AuthController(AuthService service, DatabaseRepository database, IConfiguration configuration)
        {
            _service = service;
            _database = database;
            _configuration = configuration;
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.SignIn)]
        [ValidateModelState]
        public JsonResult SignIn(
            [FromBody] SignInModel body
        )
        {
            var encryptedPasswd = Encryptor.Encrypt(_configuration[JwtConstants.SecretKeyName], body.Password!);

            var user = _database.Users.FirstOrDefault(model => model.Username == body.Username && model.Password == encryptedPasswd);
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
