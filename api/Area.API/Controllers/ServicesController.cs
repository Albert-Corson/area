using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Repositories;
using Area.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Area.API.Controllers
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
                Data = redirect.ToString()
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
        public ContentResult SignInServiceCallback(
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            if (_serviceManager.HandleServiceSignInCallbackById(HttpContext, serviceId!.Value)) {
                return Content("<h1>Success! You can now close this page!</h1>", "text/html");
            }

            Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return Content("<h1>An error has occured, please try again</h1>", "text/html");
        }
    }
}
