using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;
using Area.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbContext = Area.API.Database.DbContext;

namespace Area.API.Controllers
{
    public class WidgetsController : ControllerBase
    {
        private readonly DbContext _database;
        private readonly WidgetManagerService _widgetManager;

        public WidgetsController(DbContext database, WidgetManagerService widgetManager)
        {
            _database = database;
            _widgetManager = widgetManager;
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
                    .Include(model => model.Widgets).ThenInclude(model => model.Service)
                    .Include(model => model.Widgets).ThenInclude(model => model.Params)
                    .FirstOrDefault(model => model.Id == serviceId);

                if (service?.Id == null)
                    throw new NotFoundHttpException("Service not found");

                widgets = service.Widgets?.ToList() ?? new List<WidgetModel>();
            } else {
                widgets = _database.Widgets
                    .Include(model => model.Service)
                    .Include(model => model.Params)
                    .AsQueryable().ToList();
            }

            return new ResponseModel<List<WidgetModel>> {
                Data = widgets
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Widgets.GetMyWidgets)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetMyWidgets(
            [FromQuery] [Range(1, 2147483647)] int? serviceId
        )
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);

            var user = _database.Users
                .Include(model => model.Widgets).ThenInclude(userWidget => userWidget.Widget).ThenInclude(widget => widget!.Service)
                .Include(model => model.Widgets).ThenInclude(userWidget => userWidget.Widget).ThenInclude(widget => widget!.Params)
                .Include(model => model.WidgetParams)
                .FirstOrDefault(model => model.Id == userId)!;

            var userToWidgets = user.Widgets;

            List<WidgetModel> widgets;
            if (serviceId != null && userToWidgets != null) {
                widgets = userToWidgets
                    .Where(model => model.Widget!.ServiceId == serviceId)
                    .Select(model => model.Widget).ToList()!;
            } else if (userToWidgets != null) {
                widgets = userToWidgets.Select(model => model.Widget).ToList()!;
            } else {
                widgets = new List<WidgetModel>();
            }

            foreach (var widget in widgets) {
                widget.Params = WidgetManagerService.BuildUserWidgetCallParams(user.WidgetParams!.Where(model => model.WidgetId == widget.Id)!, widget.Params!);
            }

            return new ResponseModel<List<WidgetModel>> {
                Data = widgets
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
             return new ResponseModel<WidgetCallResponseModel> {
                 Data = _widgetManager.CallWidgetById(HttpContext, widgetId!.Value)
             };
        }

        [HttpDelete]
        [Route(RoutesConstants.Widgets.UnsubscribeWidget)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult UnsubscribeWidget(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);
            var user = _database.Users
                .Include(model => model.WidgetParams)
                .First(model => model.Id == userId);

            _database.Remove(new UserWidgetModel {
                UserId = userId,
                WidgetId = widgetId
            });

            var userWidgetParams = user.WidgetParams?.Where(model => model.WidgetId == widgetId);
            if (userWidgetParams != null) {
                foreach (var param in userWidgetParams) {
                    _database.Remove(param);
                }
            }

            _database.SaveChanges();

            return StatusModel.Success();
        }

        [HttpPost]
        [Route(RoutesConstants.Widgets.SubscribeWidget)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult SubscribeWidget(
            [FromRoute] [Required] [Range(1, 2147483647)] int? widgetId
        )
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);

            var widget = _database.Widgets.FirstOrDefault(model => model.Id == widgetId);
            if (widget == null)
                throw new NotFoundHttpException();

            var user = _database.Users.First(model => model.Id == userId);

            user.Widgets ??= new List<UserWidgetModel>();

            user.Widgets.Add(new UserWidgetModel {
                UserId = userId,
                WidgetId = widgetId
            });

            _database.SaveChanges();

            return StatusModel.Success();
        }
    }
}
