using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dashboard.API.Attributes;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.ManyToMany;
using Dashboard.API.Repositories;
using Dashboard.API.Services;
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
                    .Include(model => model.Widgets!
                        .Select(widgetModel => new {
                            widgetModel.Service,
                            widgetModel.DefaultParams
                        }))
                    .FirstOrDefault(model => model.Id == serviceId);

                if (service?.Id == null)
                    throw new NotFoundHttpException("Service not found");

                widgets = service.Widgets?.ToList() ?? new List<WidgetModel>();
            } else {
                widgets = _database.Widgets
                    .Include(model => model.Service)
                    .Include(model => model.DefaultParams)
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
            var userId = AuthService.GetUserIdFromPrincipal(User);

            if (userId == null)
                throw new UnauthorizedHttpException();

            var userToWidgets = _database.Users
                .Include(user => user.Widgets).ThenInclude(userWidget => userWidget.Widget).ThenInclude(widget => widget.Service)
                .Include(user => user.Widgets).ThenInclude(userWidget => userWidget.Widget).ThenInclude(widget => widget.DefaultParams)
                .FirstOrDefault(model => model.Id == userId)
                ?.Widgets;

            List<WidgetModel?> widgets;
            if (serviceId != null && userToWidgets != null) {
                widgets = userToWidgets
                    .Where(model => model.Widget!.ServiceId == serviceId)
                    .Select(model => model.Widget).ToList();
            } else if (userToWidgets != null) {
                widgets = userToWidgets.Select(model => model.Widget).ToList();
            } else {
                widgets = new List<WidgetModel?>();
            }

            return new ResponseModel<List<WidgetModel?>> {
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
            var userId = AuthService.GetUserIdFromPrincipal(User);
            if (userId == null)
                throw new UnauthorizedHttpException();


            var user = _database.Users
                .Include(model => model.WidgetParams)
                .FirstOrDefault(model => model.Id == userId);
            if (user == null)
                throw new NotFoundHttpException("This access token may belong to a deleted user");

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
            if (userId == null)
                throw new UnauthorizedHttpException();

            var widget = _database.Widgets.FirstOrDefault(model => model.Id == widgetId);
            if (widget == null)
                throw new NotFoundHttpException();

            var user = _database.Users.First(model => model.Id == userId);
            if (user == null)
                throw new NotFoundHttpException("This access token may belong to a deleted user");

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
