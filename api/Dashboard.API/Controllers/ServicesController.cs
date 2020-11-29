using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Table;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class ServicesController : ControllerBase
    {
        [HttpGet]
        [Route(RoutesConstants.Services.GetServices)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetServices()
        {
            // TODO: get a real list of services

            return new ResponseModel<List<ServiceModel>> {
                Data = new List<ServiceModel>()
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Services.GetMyServices)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetMyService()
        {
            // TODO: get a real list of services used by the user

            return new ResponseModel<List<ServiceModel>> {
                Data = new List<ServiceModel>()
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
            return new ResponseModel<ServiceModel> {
                Data = new ServiceModel {
                    Id = 0,
                    Name = "" // TODO: get the real associated service
                }
            };
            return StatusModel.Failed("error message");
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
