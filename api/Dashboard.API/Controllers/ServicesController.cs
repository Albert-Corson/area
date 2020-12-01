using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Table;
using Dashboard.API.Repositories;
using Dashboard.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.API.Controllers
{
    public class ServicesController : ControllerBase
    {
        private readonly DatabaseRepository _database;

        public ServicesController(DatabaseRepository database)
        {
            _database = database;
        }

        [HttpGet]
        [Route(RoutesConstants.Services.GetServices)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetServices()
        {
            return new ResponseModel<List<ServiceModel>> {
                Data = _database.Services.AsQueryable().ToList()
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Services.GetMyServices)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetMyService()
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);

            if (userId == null)
                throw new UnauthorizedHttpException();

            var user = _database.Users
                    .Include(model => model.ServiceTokens)
                    .ThenInclude(model => model.Service)
                    .FirstOrDefault(model => model.Id == userId);

            if (user == null)
                throw new NotFoundHttpException();

            List<ServiceModel> services = new List<ServiceModel>();
            if (user.ServiceTokens != null)
                services.AddRange(user.ServiceTokens.Select(tokensModel => tokensModel.Service!));

            return new ResponseModel<List<ServiceModel>> {
                Data = services
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Services.GetService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult GetService(
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            var service = _database.Services.FirstOrDefault(model => model.Id == serviceId);

            if (service == null)
                throw new NotFoundHttpException();

            return new ResponseModel<ServiceModel> {
                Data = service
            };
        }

        [HttpPost]
        [Route(RoutesConstants.Services.SignInService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult SignInService(
            // TODO: somehow get service specific credentials
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            // TODO: login to service

            return StatusModel.Success();
            return StatusModel.Failed("error message");
        }

        [HttpDelete]
        [Route(RoutesConstants.Services.SignOutService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult SignOutService(
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            // TODO: logout of a user from a service

            return StatusModel.Failed("error message");
            return StatusModel.Success();
        }
    }
}
