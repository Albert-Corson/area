using System;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class DefaultController : ControllerBase
    {
        [Route(RoutesConstants.Default.Error)]
        public JsonResult Error()
        {
            throw new NotFoundHttpException();
        }

        [Route(RoutesConstants.Default.AboutDotJson)]
        public JsonResult AboutDotJson()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
