using System;
using System.Linq;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Response;
using Dashboard.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.API.Controllers
{
    public class DefaultController : ControllerBase
    {
        private readonly DatabaseRepository _database;

        public DefaultController(DatabaseRepository database)
        {
            _database = database;
        }

        [Route(RoutesConstants.Default.Error)]
        public JsonResult Error()
        {
            throw new NotFoundHttpException();
        }

        [Route(RoutesConstants.Default.AboutDotJson)]
        public JsonResult AboutDotJson()
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4() + ":" + HttpContext.Connection.RemotePort;

            var serviceModels = _database.Services
                .Include(model => model.Widgets).ThenInclude(model => model.DefaultParams)
                .AsNoTracking()
                .ToList();

            foreach (var widget in serviceModels.Where(service => service.Widgets != null).SelectMany(service => service.Widgets)) {
                widget.Id = null;
                widget.RequiresAuth = null;
            }

            var aboutDotJson = new AboutDotJsonModel {
                Client = new AboutDotJsonModel.ClientModel {
                    Host = clientIp
                },
                Server = new AboutDotJsonModel.ServerModel {
                    CurrentTime = DateTime.Now.Ticks,
                    Services = serviceModels
                }
            };

            return new JsonResult(aboutDotJson);
        }
    }
}
