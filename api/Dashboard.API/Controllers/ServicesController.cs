using System.Collections;
using System.ComponentModel.DataAnnotations;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
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
        public JsonResult ServicesGet()
        {
            var serviceModels = new ArrayList(); // TODO: get a real list of services

            return new ResponseModel<ArrayList> {
                Data = serviceModels
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Services.ServiceIdLogout)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult ServicesServiceIdDelete(
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            // TODO: logout of a user from a service

            return StatusModel.Failed("error message");
            return StatusModel.Success();
        }

        [HttpGet]
        [Route(RoutesConstants.Services.ServiceIdGetService)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult ServicesServiceIdGet(
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
        [Route(RoutesConstants.Services.ServiceIdLogin)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult ServicesServiceIdPost(
            [FromBody] LoginModel body,
            [FromRoute] [Required] [Range(1, 2147483647)] int? serviceId
        )
        {
            // TODO: login to service

            return StatusModel.Success();
            return StatusModel.Failed("error message");
        }
    }
}
