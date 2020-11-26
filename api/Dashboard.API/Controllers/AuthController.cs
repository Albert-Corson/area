using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.API.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("/auth/refresh")]
        [ValidateModelState]
        public JsonResult AuthRefreshPost(
            [FromBody] RefreshTokenModel body
        )
        {
            // TODO: check if the refresh_token is valid

            return new ResponseModel<UserTokenModel> {
                Data = {
                    AccessToken = "", // TODO: get a new access_token
                    ExpiresIn = 42
                }
            };
            return StatusModel.Failed("error message"); // TODO: if failed
        }

        [HttpDelete]
        [Route("/auth/revoke")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult AuthRevokeDelete()
        {
            // TODO: revoke the credentials, check with Adrien how this is supposed to be done
            return StatusModel.Failed("error message"); // TODO: if failed
            return StatusModel.Success();
        }

        [HttpPost]
        [Route("/auth/token")]
        [ValidateModelState]
        public JsonResult AuthTokenPost(
            [FromBody] LoginModel body
        )
        {
            return new ResponseModel<UserTokenModel> {
                Data = {
                    RefreshToken = Startup.Configuration[AppConstants.ValidIssuer], // TODO: get the users' tokens
                    AccessToken = GenerateAccessToken("some username"),
                    ExpiresIn = TimeSpan.FromDays(14).Ticks
                }
            };
            return StatusModel.Failed("error message"); // TODO: if failed
        }

        private static string GenerateAccessToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration["SECRET_SALT"]));
            const string algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.NameId, username),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToLongTimeString()),
                new Claim(JwtRegisteredClaimNames.Iss, Startup.Configuration["ValidIssuer"]),
            };

            var token = new JwtSecurityToken(
                issuer: Startup.Configuration["ValidIssuer"],
                audience: Startup.Configuration["ValidAudience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(14),
                signingCredentials: signingCredentials
            );
            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenJson;
        }
    }
}
