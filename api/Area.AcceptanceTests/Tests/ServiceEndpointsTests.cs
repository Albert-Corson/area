using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Fixtures;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Models.Responses;
using Area.AcceptanceTests.Utilities;
using Newtonsoft.Json;
using Xunit;
using Xunit.Priority;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(AreaCollection))]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ServiceEndpointsTests : IClassFixture<AuthenticatedTestsFixture>, IClassFixture<ServiceModel>
    {
        private readonly ServiceModel _service;
        private readonly AuthenticatedTestsFixture _authenticatedTestsFixture;

        private AreaApi AreaApi => _authenticatedTestsFixture.AreaApi;

        public ServiceEndpointsTests(AuthenticatedTestsFixture authenticatedTestsFixture, ServiceModel service)
        {
            _authenticatedTestsFixture = authenticatedTestsFixture;
            _service = service;
        }

        [Fact, Priority(1)]
        public async Task GetServices()
        {
            var response = await AreaApi.GetServices();

            AssertExtension.SuccessfulApiResponse(response);

            var rand = new Random();
            var service = response.Content.Data!.ElementAt(rand.Next(0, response.Content.Data!.Count() - 1));
            _service.Copy(service);
        }

        [Fact, Priority(2)]
        public async Task GetServiceById()
        {
            var response = await AreaApi.GetServiceById(_service.Id);

            AssertExtension.SuccessfulApiResponse(response);
            Assert.Equal(_service.Id, response.Content.Data!.Id);
            Assert.Equal(_service.Name, response.Content.Data!.Name);
        }

        [Fact, Priority(3)]
        public async Task SignInServiceById()
        {
            var form = new ExternalAuthModel {
                State = "test",
                RedirectUrl = "http://google.fr"
            };
            var response = await AreaApi.SignInServiceById(_service.Id, form);

            Assert.True(response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.OK);

            var queryParams = HttpUtility.ParseQueryString(response.Headers.Location.Query);
            var state = queryParams.Get("state");

            if (response.Headers.Location.ToString().StartsWith(form.RedirectUrl)) {
                Assert.Equal(form.State, state);
                Assert.Equal("true", queryParams.Get("successful"));
            } else {
                var recoveredForm = JsonConvert.DeserializeObject<ExternalAuthModel>(HttpUtility.UrlDecode(state));
                Assert.Equal(form.State, recoveredForm.State);
                Assert.Equal(new Uri(form.RedirectUrl), new Uri(recoveredForm.RedirectUrl));
            }
        }

        [Fact, Priority(10)]
        public async Task SignOutFromParentService()
        {
            var response = await AreaApi.SignOutServiceById(_service.Id);

            AssertExtension.SuccessfulApiResponse(response);
        }
    }
}