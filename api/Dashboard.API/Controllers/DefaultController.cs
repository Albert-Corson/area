using Dashboard.API.Exceptions.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("")]
    public class Default : Controller
    {
        public IActionResult Index()
        {
            throw new NotFoundHttpException("Resource not found");
        }
    }
}
