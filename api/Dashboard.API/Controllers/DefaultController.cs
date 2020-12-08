using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Constants;
using Dashboard.API.Models;
using Dashboard.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            return new StatusModel {
                Error = ReasonPhrases.GetReasonPhrase(Response.StatusCode),
                Successful = false
            };
        }

        [Route(RoutesConstants.Default.AboutDotJson)]
        public JsonResult AboutDotJson()
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4() + ":" + HttpContext.Connection.RemotePort;

            var serviceModels = _database.Services
                .Include(model => model.Widgets).ThenInclude(model => model.Params)
                .AsNoTracking()
                .ToList();

            foreach (var widget in serviceModels.Where(service => service.Widgets != null).SelectMany(service => service.Widgets)) {
                widget.Id = null;
                widget.RequiresAuth = null;
                widget.Service = null;
            }

            var services = serviceModels.Select(service => new AboutDotJsonModel.ServiceModel {
                Name = service.Name,
                Widgets = service.Widgets?.Select(widget =>  new AboutDotJsonModel.WidgetModel {
                    Name = widget.Name,
                    Description = widget.Description,
                    Params = widget.Params?.Select(model => new AboutDotJsonModel.WidgetParamModel {
                        Name = model.Name,
                        Type = model.Type
                    }) ?? new List<AboutDotJsonModel.WidgetParamModel>()
                }) ?? new List<AboutDotJsonModel.WidgetModel>()
            }).ToList();

            var aboutDotJson = new AboutDotJsonModel {
                Client = new AboutDotJsonModel.ClientModel {
                    Host = clientIp
                },
                Server = new AboutDotJsonModel.ServerModel {
                    CurrentTime = DateTime.Now.Ticks,
                    Services = services
                }
            };

            return new JsonResult(aboutDotJson, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
