using Dashboard.API.Exceptions.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("/Error")]
    public class Default : Controller
    {
        public IActionResult Index()
        {
            throw new NotFoundHttpException();
        }
    }
}
