using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Models.Requests;
using Newtonsoft.Json;
using Xunit;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(AreaCollection))]
    public class FacebookAuthenticationTests
    {
        [Fact]
        public async Task SignInWithFacebook()
        {
            var areaApi = new AreaApi();
            var form = new ExternalAuthModel {
                State = "test abcd",
                RedirectUrl = "http://google.fr"
            };

            var response = await areaApi.SignInWithFacebook(form);

            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.StartsWith("https://www.facebook.com/dialog/oauth", response.Headers.Location.ToString());

            var queryParams = HttpUtility.ParseQueryString(response.Headers.Location.Query);
            var state = HttpUtility.UrlDecode(queryParams.Get("state"));
            var recoveredForm = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            Assert.Equal(new Uri(form.RedirectUrl), new Uri(recoveredForm.RedirectUrl));
            Assert.Equal(form.State, recoveredForm.State);
        }
    }
}