using System;
using System.Threading.Tasks;
using System.Web;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Xunit;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(AreaCollection))]
    public class ExternalAuthenticationTests
    {
        private readonly AreaApi _areaApi = new AreaApi();

        private readonly ExternalAuthModel _form = new ExternalAuthModel {
            State = "test abcd",
            RedirectUrl = "http://google.fr"
        };
  
        [Fact]
        public async Task SignInWithFacebook()
        {
            var query = new QueryBuilder {
                {"state", _form.State},
                {"redirect_url", _form.RedirectUrl}
            };

            var response = await _areaApi.SignInWithFacebook(query.ToString());

            AssertExtension.SuccessfulApiResponse(response);
            Assert.True(response.Content.Data!.RequiresRedirect);
            Assert.NotNull(response.Content.Data!.RedirectUrl);
            Assert.StartsWith("https://www.facebook.com/dialog/oauth", response.Content.Data!.RedirectUrl!);

            var queryParams = HttpUtility.ParseQueryString(new Uri(response.Content.Data!.RedirectUrl!).Query);
            var state = queryParams.Get("state");
            var recoveredForm = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            Assert.Equal(_form.State, recoveredForm.State);
            Assert.Equal(new Uri(_form.RedirectUrl), new Uri(recoveredForm.RedirectUrl));
        }

        [Fact]
        public async Task SignInWithGoogle()
        {
            var query = new QueryBuilder {
                {"state", _form.State},
                {"redirect_url", _form.RedirectUrl}
            };

            var response = await _areaApi.SignInWithGoogle(query.ToString());

            AssertExtension.SuccessfulApiResponse(response);
            Assert.True(response.Content.Data!.RequiresRedirect);
            Assert.NotNull(response.Content.Data!.RedirectUrl);
            Assert.StartsWith("https://accounts.google.com/o/oauth2/auth", response.Content.Data!.RedirectUrl!);

            var queryParams = HttpUtility.ParseQueryString(new Uri(response.Content.Data!.RedirectUrl!).Query);
            var state = queryParams.Get("state");
            var recoveredForm = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            Assert.Equal(_form.State, recoveredForm.State);
            Assert.Equal(new Uri(_form.RedirectUrl), new Uri(recoveredForm.RedirectUrl));
        }

        [Fact]
        public async Task SignInWithMicrosoft()
        {
            var query = new QueryBuilder {
                {"state", _form.State},
                {"redirect_url", _form.RedirectUrl}
            };

            var response = await _areaApi.SignInWithMicrosoft(query.ToString());

            AssertExtension.SuccessfulApiResponse(response);
            Assert.True(response.Content.Data!.RequiresRedirect);
            Assert.NotNull(response.Content.Data!.RedirectUrl);
            Assert.StartsWith("https://login.microsoftonline.com/common/oauth2/v2.0/authorize", response.Content.Data!.RedirectUrl!);

            var queryParams = HttpUtility.ParseQueryString(new Uri(response.Content.Data!.RedirectUrl!).Query);
            var state = queryParams.Get("state");
            var recoveredForm = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            Assert.Equal(_form.State, recoveredForm.State);
            Assert.Equal(new Uri(_form.RedirectUrl), new Uri(recoveredForm.RedirectUrl));
        }
    }
}