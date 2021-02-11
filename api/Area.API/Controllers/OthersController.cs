using System;
using System.Linq;
using Area.API.Constants;
using Area.API.Models;
using Area.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Controllers
{
    [SwaggerTag("Other informational endpoints")]
    public class OthersController : ControllerBase
    {
        private readonly ServiceRepository _serviceRepository;

        public OthersController(ServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [Route(RoutesConstants.Error)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public StatusModel Error()
        {
            return new StatusModel {
                Error = ReasonPhrases.GetReasonPhrase(Response.StatusCode),
                Successful = false
            };
        }

        [HttpGet]
        [Route(RoutesConstants.AboutDotJson)]
        [SwaggerOperation(
            Summary = "General information about the API's content",
            Description = "## Get general information about the API's content such as the list of all services and widgets (and more TO BE DEFINED)"
        )]
        public AboutDotJsonModel AboutDotJson()
        {
            // TODO: rework this endpoint to return useful information
            var clientIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4() + ":" + HttpContext.Connection.RemotePort;

            var serviceModels = _serviceRepository.GetServices(true).ToList();

            var services = serviceModels.Select(service => new AboutDotJsonModel.ServiceModel {
                    Name = service.Name,
                    Widgets = service.Widgets.Select(widget => new AboutDotJsonModel.WidgetModel {
                        Name = widget.Name,
                        Description = widget.Description,
                        Params = widget.Params.Select(model => new AboutDotJsonModel.WidgetParamModel {
                            Name = model.Name,
                            Type = model.Type
                        })
                    })
                })
                .ToList();

            return new AboutDotJsonModel {
                Client = new AboutDotJsonModel.ClientModel {
                    Host = clientIp
                },
                Server = new AboutDotJsonModel.ServerModel {
                    CurrentTime = DateTime.Now.Ticks,
                    Services = services
                }
            };
        }
    }
}