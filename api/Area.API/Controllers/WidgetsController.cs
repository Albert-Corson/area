using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
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
    [SwaggerTag("Widget-related endpoints")]
    public class WidgetsController : ControllerBase
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly UserRepository _userRepository;
        private readonly WidgetManagerService _widgetManager;
        private readonly WidgetRepository _widgetRepository;

        public WidgetsController(WidgetManagerService widgetManager, WidgetRepository widgetRepository,
            ServiceRepository serviceRepository, UserRepository userRepository)
        {
            _widgetManager = widgetManager;
            _widgetRepository = widgetRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
        }

        [HttpGet(RouteConstants.Widgets.GetWidgets)]
        [SwaggerOperation(
            Summary = "List all widgets",
            Description =
                "## List all widgets. Optionally, you can get the widgets from one particular service. **The values of the parameters (`params`) are the widgets' default ones**"
        )]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "The `serviceId` doesn't have a correspond service")]
        public ResponseModel<List<WidgetModel>> GetWidgets(
            [FromQuery] [Range(1, int.MaxValue)] [SwaggerParameter("A service's ID, to filter the results by.")]
            int? serviceId
        )
        {
            IEnumerable<WidgetModel> widgets;

            if (serviceId != null) {
                if (!_serviceRepository.ServiceExists(serviceId.Value))
                    throw new NotFoundHttpException("This service does not exist");
                widgets = _widgetRepository.GetWidgetsByService(serviceId.Value, true);
            } else {
                widgets = _widgetRepository.GetWidgets(true);
            }

            return new ResponseModel<List<WidgetModel>> {
                Data = widgets.ToList()
            };
        }

        [HttpGet(RouteConstants.Widgets.GetMyWidgets)]
        [SwaggerOperation(
            Summary = "List a user's widgets",
            Description =
                "List the user's widgets. Optionally, you can get the widgets from one particular service. **The values of the parameters (`params`) are the ones the user has previously used in call to `"
                + RouteConstants.Widgets.CallWidget + "`**"
        )]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "The `serviceId` doesn't have a correspond service")]
        public ResponseModel<List<WidgetModel>> GetMyWidgets(
            [FromQuery] [Range(1, int.MaxValue)] [SwaggerParameter("A service's ID, to filter the results by.")]
            int? serviceId
        )
        {
            User.TryGetUserId(out var userId);

            List<WidgetModel> widgets;
            if (serviceId != null) {
                if (!_serviceRepository.ServiceExists(serviceId.Value))
                    throw new NotFoundHttpException("This service does not exist");
                widgets = _widgetRepository.GetUserWidgetsByService(userId, serviceId.Value).ToList();
            } else {
                widgets =  _widgetRepository.GetUserWidgets(userId, true).ToList();
            }

            var user = _userRepository.GetUser(userId, includeChildren: true);
            var widgetParams = user!.WidgetParams.ToList();

            foreach (var widget in widgets) {
                var currentParam = widgetParams.Where(model => model.Param.WidgetId == widget.Id);
                widget.Params = WidgetManagerService.BuildUserWidgetCallParams(currentParam, widget.Params!);
                widget.RequiresAuth = user.ServiceTokens.FirstOrDefault(model => model.ServiceId == widget.ServiceId) != null;
            }

            return new ResponseModel<List<WidgetModel>> {
                Data = widgets
            };
        }

        [HttpGet(RouteConstants.Widgets.CallWidget)]
        [SwaggerOperation(
            Summary = "Call a widget",
            Description = @"## Call the widget's corresponding API.
## The query parameters will be used to override/complete the widget's `params` values, i.e.:**`paramName`=`value`**.
## The API's request result is interpolated into a its corresponding data scheme (inheriting from `WidgetCallResponse`) and returned"
        )]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "The widget doesn't exist")]
        public ResponseModel<WidgetCallResponseModel> CallWidget(
            [FromRoute] [Required] [Range(1, int.MaxValue)] [SwaggerParameter("The widget's ID")]
            int? widgetId
        )
        {
            return new ResponseModel<WidgetCallResponseModel> {
                Data = _widgetManager.CallWidgetById(HttpContext, widgetId!.Value)
            };
        }

        [HttpDelete(RouteConstants.Widgets.UnsubscribeWidget)]
        [SwaggerOperation(
            Summary = "Unsubscribe the user to a widget",
            Description = "## Remove a widget to the user's subscriptions"
        )]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "The widget doesn't exist or the user isn't subscribed to it")]
        public StatusModel UnsubscribeWidget(
            [FromRoute] [Required] [Range(1, int.MaxValue)] [SwaggerParameter("The widget's ID")]
            int? widgetId
        )
        {
            User.TryGetUserId(out var userId);

            if (!_userRepository.RemoveWidgetSubscription(userId, widgetId!.Value))
                throw new NotFoundHttpException("The widget doesn't exist or the user is not subscribed");
            return StatusModel.Success();
        }

        [HttpPost(RouteConstants.Widgets.SubscribeWidget)]
        [SwaggerOperation(
            Summary = "Subscribe the user to a widget",
            Description = "## Add a widget to the user's subscriptions"
        )]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "The widget doesn't exist")]
        public StatusModel SubscribeWidget(
            [FromRoute, Required, Range(1, int.MaxValue)]
            [SwaggerParameter("The widget's ID")]
            int? widgetId
        )
        {
            User.TryGetUserId(out var userId);

            if (!_userRepository.AddWidgetSubscription(userId, widgetId!.Value))
                throw new NotFoundHttpException("This widget does not exist");

            return StatusModel.Success();
        }
    }
}