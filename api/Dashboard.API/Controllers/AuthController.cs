using Dashboard.API.Attributes;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("/auth/refresh")]
        [ValidateModelState]
        public IActionResult AuthRefreshPost([FromBody] RefreshTokenModel body)
        {
            var responseModel = new ResponseModel<UserTokenModel> {
                Data = {
                    AccessToken = body.RefreshToken,
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
        // public IActionResult AuthRevokeDelete()
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
        // public IActionResult AuthTokenPost([FromBody]Credentials body)
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
