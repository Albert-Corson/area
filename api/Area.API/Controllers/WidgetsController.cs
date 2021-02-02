using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Repositories;
using Area.API.Services;
using Area.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Area.API.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Route(RoutesConstants.Widgets.GetWidgets)]
        [ValidateModelState]
        public ResponseModel<List<WidgetModel>> GetWidgets(
            [FromQuery] [Range(1, 2147483647)] int? serviceId
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

        [HttpGet]
        [Route(RoutesConstants.Widgets.GetMyWidgets)]
        public ResponseModel<List<WidgetModel>> GetMyWidgets(
            [FromQuery] [Range(1, 2147483647)] int? serviceId
        )
        {
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);

            var widgets = serviceId != null
                ? _widgetRepository.GetUserWidgetsByService(userId!.Value, serviceId.Value).ToList()
                : _widgetRepository.GetUserWidgets(userId!.Value, true).ToList();

            var user = _userRepository.GetUser(userId);
            var widgetParams = user!.WidgetParams.ToList();

            foreach (var widget in widgets) {
                var currentParam = widgetParams.Where(model => model.WidgetId == widget.Id);
                widget.Params = WidgetManagerService.BuildUserWidgetCallParams(currentParam, widget.Params!);
                widget.RequiresAuth = user.ServiceTokens.FirstOrDefault(model => model.ServiceId == widget.ServiceId) != null;
            }

            return new ResponseModel<List<WidgetModel>> {
                Data = widgets
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Widgets.CallWidget)]
        [ValidateModelState]
        public ResponseModel<WidgetCallResponseModel> CallWidget(
            [FromRoute] [Required] [Range(1, 2147483647)]
            int? widgetId
        )
        {
            return new ResponseModel<WidgetCallResponseModel> {
                Data = _widgetManager.CallWidgetById(HttpContext, widgetId!.Value)
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Widgets.UnsubscribeWidget)]
        [ValidateModelState]
        public StatusModel UnsubscribeWidget(
            [FromRoute] [Required] [Range(1, 2147483647)]
            int? widgetId
        )
        {
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);

            if (!_userRepository.RemoveWidgetSubscription(userId!.Value, widgetId!.Value))
                throw new NotFoundHttpException("The user is not subscribed to this widget");
            return StatusModel.Success();
        }

        [HttpPost]
        [Route(RoutesConstants.Widgets.SubscribeWidget)]
        [ValidateModelState]
        public StatusModel SubscribeWidget(
            [FromRoute] [Required] [Range(1, 2147483647)]
            int? widgetId
        )
        {
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);

            if (!_widgetRepository.WidgetExists(widgetId!.Value))
                throw new NotFoundHttpException("This widget does not exist");

            _userRepository.AddWidgetSubscription(userId!.Value, widgetId!.Value);
            return StatusModel.Success();
        }
    }
}