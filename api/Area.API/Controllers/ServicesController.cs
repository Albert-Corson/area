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
using Area.API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Area.API.Controllers
{
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

        [HttpGet]
        [Route(RoutesConstants.Services.GetServices)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ResponseModel<List<ServiceModel>> GetServices()
        {
            return new ResponseModel<List<ServiceModel>> {
                Data = _serviceRepository.GetServices().ToList()
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Services.GetMyServices)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ResponseModel<List<ServiceModel>> GetMyService()
        {
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);

            var services = _serviceRepository.GetServicesByUser(userId!.Value);

            return new ResponseModel<List<ServiceModel>> {
                Data = services
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Services.GetService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public ResponseModel<ServiceModel> GetService(
            [FromRoute] [Required] [Range(1, 2147483647)]
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

        [HttpPost]
        [Route(RoutesConstants.Services.SignInService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public ResponseModel<string?> SignInService(
            [FromRoute] [Required] [Range(1, 2147483647)]
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

        [HttpDelete]
        [Route(RoutesConstants.Services.SignOutService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public StatusModel SignOutService(
            [FromRoute] [Required] [Range(1, 2147483647)]
            int? serviceId
        )
        {
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);

            _userRepository.RemoveServiceCredentials(userId!.Value, serviceId!.Value);
            return StatusModel.Success();
        }

        [HttpGet]
        [Route(RoutesConstants.Services.SignInServiceCallback)]
        [ValidateModelState]
        public ContentResult SignInServiceCallback(
            [FromRoute] [Required] [Range(1, 2147483647)]
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