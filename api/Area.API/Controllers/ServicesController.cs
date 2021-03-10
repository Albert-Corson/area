using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Repositories;
using Area.API.Services;
using Area.API.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerTag("Service-related endpoints")]
    public class ServicesController : ControllerBase
    {
        private readonly ServiceManager _serviceManager;
        private readonly ServiceRepository _serviceRepository;
        private readonly UserRepository _userRepository;

        public ServicesController(ServiceManager serviceManager, ServiceRepository serviceRepository,
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
            if (!User.TryGetUserId(out var userId))
                throw new InternalServerErrorHttpException();

            var services = _serviceRepository.GetServicesByUser(userId);

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

        [HttpGet(RouteConstants.Services.SignInService)]
        [SwaggerOperation(
            Summary = "Sign-in a user to a service",
            Description =
                @"## Sign-in the user to a service.
## If the service doesn't have sign-in capabilities, an empty success response is returned (a.k.a without `data`).
## Otherwise an authentication URL is returned as `data` to redirect the user to"
        )]
        public async Task<ResponseModel<AuthenticationRedirectModel>> SignInService(
            [FromRoute] [Required] [Range(1, int.MaxValue)] [SwaggerParameter("Service's ID")]
            int? serviceId,

            [Required, FromQuery(Name = "redirect_url")]
            [SwaggerParameter("The URL to redirect the user to once the operation is completed")]
            string? redirectUrl,

            [FromQuery(Name = "state")]
            [SwaggerParameter("A freely-defined value that will sent back to the client")]
            string? clientState
        )
        {
            var state = new ServiceAuthStateModel {
                State = clientState,
                RedirectUrl = redirectUrl!
            };

            User.TryGetUserId(out state.UserId);

            if (_serviceRepository.GetService(serviceId!.Value) == null)
                throw new NotFoundHttpException("The service does not exist");

            if (!_serviceManager.TryGetServiceById(serviceId.Value, out var service)) {
                return new ResponseModel<AuthenticationRedirectModel> {
                    Data = new AuthenticationRedirectModel()
                };
            }

            var serviceUri = await service!.GetSignInUrlAsync(JsonConvert.SerializeObject(state));

            return new ResponseModel<AuthenticationRedirectModel> {
                Data = new AuthenticationRedirectModel {
                    RedirectUrl = serviceUri.AbsoluteUri
                }
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
            if (!User.TryGetUserId(out var userId))
                throw new InternalServerErrorHttpException();

            _userRepository.RemoveServiceCredentials(userId, serviceId!.Value);
            return StatusModel.Success();
        }

        [HttpGet(RouteConstants.Services.SignInServiceCallback)]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Produces("text/html")]
        public async Task<IActionResult> SignInServiceCallback(
            [FromRoute] [Required] [Range(1, int.MaxValue)]
            int? serviceId,
            [FromQuery] [Required] string? state,
            [FromQuery] [Required] string? code
        )
        {
            ServiceAuthStateModel serviceAuthState;

            try {
                serviceAuthState = JsonConvert.DeserializeObject<ServiceAuthStateModel>(state!);
            } catch {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return Content("<h1>An unexpected error has occured, please try again later</h1>", "text/html");
            }

            bool failed;
            var redirectUrl = new UriBuilder(serviceAuthState.RedirectUrl);
            var queryParams = HttpUtility.ParseQueryString(redirectUrl.Query);

            if (serviceAuthState.State != null)
                queryParams["state"] = serviceAuthState.State;

            try {
                failed = !await _serviceManager.HandleServiceSignInCallbackById(serviceId!.Value, serviceAuthState.UserId, code!);
            } catch {
                failed = true;
            }

            if (failed) {
                queryParams["successful"] = "false";
                queryParams["error"] = "Unable to sign-in, please try again later.";
            } else {
                queryParams["successful"] = "true";
            }

            redirectUrl.Query = queryParams.ToString();

            return new RedirectResult(redirectUrl.ToString());
        }
    }
}