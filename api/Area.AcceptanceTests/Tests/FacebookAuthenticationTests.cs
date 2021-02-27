using System;
using System.Net;
using System.Threading.Tasks;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Models.Requests;
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
                RedirectUrl = new Uri("http://google.fr")
            };

            var response = await areaApi.SignInWithFacebook(form);

            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.StartsWith("https://www.facebook.com/dialog/oauth", response.Headers.Location.ToString());
        }
    }
}