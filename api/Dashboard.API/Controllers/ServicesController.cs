using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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
        private readonly ServiceManagerService _serviceManager;

        public ServicesController(DatabaseRepository database, ServiceManagerService serviceManager)
        {
            _database = database;
            _serviceManager = serviceManager;
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
            var user = _database.Users
                .Include(model => model.Widgets).ThenInclude(model => model.Widget).ThenInclude(model => model!.Service)
                .First(model => model.Id == userId);

            List<ServiceModel> services = new List<ServiceModel>();
            foreach (var widget in user.Widgets!) {
                if (services.Find(model => model.Id == widget.Widget!.ServiceId) != null)
                    continue;
                services.Add(widget.Widget!.Service!);
            }

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
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            var redirect = _serviceManager.SignInServiceById(HttpContext, serviceId!.Value);

            if (redirect == null)
                return StatusModel.Success();

            Response.StatusCode = (int) HttpStatusCode.Accepted;

            return new ResponseModel<string> {
                Data = redirect
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Services.SignOutService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult SignOutService(
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);
            var serviceTokens = _database.Users
                .Where(model => model.Id == userId)
                .Include(model => model.ServiceTokens!
                    .Where(tokensModel => tokensModel.ServiceId == serviceId))
                .SelectMany(model => model.ServiceTokens)
                .FirstOrDefault();

            if (serviceTokens != null) {
                _database.Remove(serviceTokens);
                _database.SaveChanges();
            }
            return StatusModel.Success();
        }

        [HttpGet]
        [Route(RoutesConstants.Services.SignInServiceCallback)]
        [ValidateModelState]
        public void SignInServiceCallback(
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            _serviceManager.HandleServiceSignInCallbackById(HttpContext, serviceId!.Value);
        }
    }
}
