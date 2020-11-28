using Dashboard.API.Exceptions.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/Error")]
        public JsonResult Index()
        {
            throw new NotFoundHttpException();
        }
    }
}
