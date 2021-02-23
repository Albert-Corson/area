using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Repositories;
using Area.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerTag("Service-related endpoints")]
    public class ServicesController : ControllerBase
    {
        private readonly ServiceManagerService _serviceManager;
        private readonly ServiceRepository _serviceRepository;
        private readonly UserRepository _userRepository;

        public ServicesController(ServiceManagerService serviceManager, ServiceRepository serviceRepository,
            UserRepository userRepository)
        {
            _serviceManager = serviceManager;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
        }

        [HttpGet(RouteConstants.Services.GetServices)]
        [SwaggerOperation(
            Summary = "List all services",
            Description = "## Get a list of all services available"
        )]
        public ResponseModel<List<ServiceModel>> GetServices()
        {
            return new ResponseModel<List<ServiceModel>> {
                Data = _serviceRepository.GetServices().ToList()
            };
        }

        [HttpGet(RouteConstants.Services.GetMyServices)]
        [SwaggerOperation(
            Summary = "List a user's services",
            Description = "## Get a list of all services where a user is subscribed to some of its widget(s)"
        )]
        public ResponseModel<List<ServiceModel>> GetMyService()
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);

            var services = _serviceRepository.GetServicesByUser(userId!.Value);

            return new ResponseModel<List<ServiceModel>> {
                Data = services
            };
        }

        [HttpGet(RouteConstants.Services.GetService)]
        [SwaggerOperation(
            Summary = "Get a service",
            Description = "## Get a information about a service in particular"
        )]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "The service does not exist")]
        public ResponseModel<ServiceModel> GetService(
            [FromRoute] [Required] [Range(1, int.MaxValue)]
            int? serviceId
        )
        {
            var service = _serviceRepository.GetService(serviceId!.Value);

            if (service == null)
                throw new NotFoundHttpException("This service does not exist");

            return new ResponseModel<ServiceModel> {
                Data = service
            };
        }

        [HttpPost(RouteConstants.Services.SignInService)]
        [SwaggerOperation(
            Summary = "Sign-in a user to a service",
            Description =
                @"## Sign-in the user to a service.
## If the service doesn't have sign-in capabilities, an empty success response is returned (a.k.a without `data`).
## Otherwise an authentication URL is returned as `data` to redirect the user to"
        )]
        public ResponseModel<string?> SignInService(
            [FromRoute] [Required] [Range(1, int.MaxValue)] [SwaggerParameter("Service's ID")]
            int? serviceId
        )
        {
            var redirect = _serviceManager.SignInServiceById(HttpContext, serviceId!.Value);

            if (redirect == null)
                return new ResponseModel<string?>();

            Response.StatusCode = (int) HttpStatusCode.Accepted;

            return new ResponseModel<string?> {
                Data = redirect.ToString()
            };
        }

        [HttpDelete(RouteConstants.Services.SignOutService)]
        [SwaggerOperation(
            Summary = "Sign-out a user from a service",
            Description =
                "## Sign-out the user from a service. If the service doesn't have sign-in capabilities, an empty success response is returned (a.k.a. without `data`)"
        )]
        public StatusModel SignOutService(
            [FromRoute] [Required] [Range(1, int.MaxValue)] [SwaggerParameter("Service's ID")]
            int? serviceId
        )
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);

            _userRepository.RemoveServiceCredentials(userId!.Value, serviceId!.Value);
            return StatusModel.Success();
        }

        [HttpGet(RouteConstants.Services.SignInServiceCallback)]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Produces("text/html")]
        public ContentResult SignInServiceCallback(
            [FromRoute] [Required] [Range(1, int.MaxValue)]
            int? serviceId
        )
        {
            try {
                if (_serviceManager.HandleServiceSignInCallbackById(HttpContext, serviceId!.Value))
                    return Content("<h1>Success! You can now close this page!</h1>", "text/html");
            } catch {
                // ignore
            }

            Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return Content("<h1>An error has occured, please try again later</h1>", "text/html");
        }
    }
}