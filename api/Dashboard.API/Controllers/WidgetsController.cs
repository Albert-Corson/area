using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Table;
using Dashboard.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.API.Controllers
{
    public class WidgetsController : ControllerBase
    {
        private readonly DatabaseRepository _database;

        public WidgetsController(DatabaseRepository database)
        {
            _database = database;
        }

        [HttpGet]
        [Route(RoutesConstants.Widgets.GetWidgets)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult GetWidgets(
            [FromQuery] [Range(1, 2147483647)] int? serviceId
        )
        {
            List<WidgetModel>? widgets;

            if (serviceId != null) {
                var service = _database.Services
                    .Include(model => model.Widgets!.Select(widgetModel => widgetModel.DefaultParams))
                    .FirstOrDefault(model => model.Id == serviceId);

                if (service?.Id == null)
                    throw new NotFoundHttpException("Service not found");

                widgets = service.Widgets?.ToList() ?? new List<WidgetModel>();
            } else {
                widgets = _database.Widgets
                    .Include(model => model.DefaultParams)
                    .Include(model => model.Service)
                    .AsQueryable().ToList();
            }
            return new ResponseModel<List<WidgetModel>> {
                Data = widgets.ToList()
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Widgets.GetMyWidgets)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetMyWidgets(
            [FromQuery] [Range(1, 2147483647)] int? serviceId
        )
        {
            // TODO: get the widgets associated to the current user and sort by serviceId
            var widgets = _database.Widgets.AsQueryable();

            return new ResponseModel<List<WidgetModel>> {
                Data = widgets.ToList()
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Widgets.CallWidget)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult CallWidget(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            // TODO: get widget by id and call its API!!!
            // Return type is widget specific
            return StatusModel.Failed("error message");
        }

        [HttpDelete]
        [Route(RoutesConstants.Widgets.UnsubscribeWidget)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult UnsubscribeWidget(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            // TODO: Unsubscribe use from widget

            return StatusModel.Success();
            return StatusModel.Failed("Not subscribed");
        }

        [HttpPost]
        [Route(RoutesConstants.Widgets.SubscribeWidget)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult SubscribeWidget(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            // TODO: subscribe user to widget

            return StatusModel.Success();
            return StatusModel.Failed("error message");
        }
    }
}
