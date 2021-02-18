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

        [Route(RouteConstants.Error)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public StatusModel Error()
        {
            return new StatusModel {
                Error = ReasonPhrases.GetReasonPhrase(Response.StatusCode),
                Successful = false
            };
        }

        [HttpGet(RouteConstants.AboutDotJson)]
        [SwaggerOperation(
            Summary = "General information about the API's content",
            Description = "## Get general information about the API's content"
        )]
        public ResponseModel<AboutDotJsonModel> AboutDotJson()
        {
            return new ResponseModel<AboutDotJsonModel> {
                Data = new AboutDotJsonModel {
                    Services = _serviceRepository.GetServices(true).Select(service => new AboutDotJsonModel.ServiceModel {
                        Name = service.Name,
                        Widgets = service.Widgets.Select(widget => new AboutDotJsonModel.WidgetModel {
                            Name = widget.Name,
                            Description = widget.Description,
                        })
                    })
                }
            };
        }
    }
}