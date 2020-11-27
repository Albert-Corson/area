using System.Collections;
using System.ComponentModel.DataAnnotations;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class WidgetsController : ControllerBase
    {
        [HttpGet]
        [Route(RoutesConstants.Widgets.GetWidgets)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult WidgetsGet(
            [FromQuery] [Range(1, 2147483647)] int? serviceId
        )
        {
            var widgets = new ArrayList();
            // TODO: get widget list. If serviceId != null: only get the ones where service.id == serviceId

            return new ResponseModel<ArrayList> {
                Data = widgets
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Widgets.WidgetIdUnsubscribe)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult WidgetsWidgetIdDelete(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            // TODO: Unsubscribe use from widget

            return StatusModel.Success();
            return StatusModel.Failed("Not subscribed");
        }

        [HttpGet]
        [Route(RoutesConstants.Widgets.WidgetIdGetWidget)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult WidgetsWidgetIdGet(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            // TODO: get widget by id

            return new ResponseModel<WidgetModel> {
                Data = {
                    Id = 42,
                    Name = "widget name",
                    ParentService = {
                        Id = 42,
                        Name = "service name"
                    }
                }
            };
            return StatusModel.Failed("error message");
        }

        [HttpPost]
        [Route(RoutesConstants.Widgets.WidgetIdSubscribe)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult WidgetsWidgetIdPost(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            // TODO: subscribe user to widget

            return StatusModel.Success();
            return StatusModel.Failed("error message");
        }
    }
}
