using System;
using System.Diagnostics;
using Dashboard.API.Attributes;
using Dashboard.API.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dashboard.API.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("/auth/refresh")]
        // [ValidateModelState]
        public virtual IActionResult AuthRefreshPost([Fro])
        {
            var responseModel = new ResponseModel<UserTokenModel> {
                Data = {
                    // AccessToken = body.RefreshToken ?? "NOON",
                    ExpiresIn = 42
                },
                Successful = true
            };

            //TODO: Uncomment the next line to return response 0 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(0, default(InlineResponseDefault));
            // string exampleJson = null;
            // exampleJson = "{\n  \"data\" : {\n    \"access_token\" : \"access_token\",\n    \"refresh_token\" : \"refresh_token\",\n    \"expires_in\" : 0\n  }\n}";
            //
            //             var example = exampleJson != null
            //             ? JsonConvert.DeserializeObject<InlineResponseDefault>(exampleJson)
            //             : default(InlineResponseDefault);            //TODO: Change the data returned
            return responseModel.ToJsonResult();
        }

        // [HttpDelete]
        // [Route("/auth/revoke")]
        // [Authorize(AuthenticationSchemes = BearerAuthenticationHandler.SchemeName)]
        // [ValidateModelState]
        // public virtual IActionResult AuthRevokeDelete()
        // {
        //     //TODO: Uncomment the next line to return response 0 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        //     // return StatusCode(0, default(Status));
        //     string exampleJson = null;
        //     exampleJson = "{\n  \"error\" : \"error\",\n  \"successful\" : true\n}";
        //
        //                 var example = exampleJson != null
        //                 ? JsonConvert.DeserializeObject<Status>(exampleJson)
        //                 : default(Status);            //TODO: Change the data returned
        //     return new ObjectResult(example);
        // }
        //
        // [HttpPost]
        // [Route("/auth/token")]
        // [ValidateModelState]
        // public virtual IActionResult AuthTokenPost([FromBody]Credentials body)
        // {
        //     //TODO: Uncomment the next line to return response 0 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        //     // return StatusCode(0, default(InlineResponseDefault));
        //     string exampleJson = null;
        //     exampleJson = "{\n  \"data\" : {\n    \"access_token\" : \"access_token\",\n    \"refresh_token\" : \"refresh_token\",\n    \"expires_in\" : 0\n  }\n}";
        //
        //                 var example = exampleJson != null
        //                 ? JsonConvert.DeserializeObject<InlineResponseDefault>(exampleJson)
        //                 : default(InlineResponseDefault);            //TODO: Change the data returned
        //     return new ObjectResult(example);
        // }
    }
}
