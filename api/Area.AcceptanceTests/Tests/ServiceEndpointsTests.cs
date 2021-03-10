using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Fixtures;
using Area.AcceptanceTests.Models;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Models.Responses;
using Area.AcceptanceTests.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
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
                State = "test to be encoded",
                RedirectUrl = "http://google.fr"
            };

            var query = new QueryBuilder {
                {"state", form.State},
                {"redirect_url", form.RedirectUrl}
            };

            var response = await AreaApi.SignInServiceById(_service.Id, query.ToString());
            var withAuthResponse = await AreaApi.SignInServiceById(1, query.ToString());

            AssertExtension.SuccessfulApiResponse(response);
            AssertExtension.SuccessfulApiResponse(withAuthResponse);
            Assert.NotNull(withAuthResponse.Content.Data!.RedirectUrl);
            Assert.True(withAuthResponse.Content.Data!.RequiresRedirect);

            var queryParams = HttpUtility.ParseQueryString(new Uri(withAuthResponse.Content.Data!.RedirectUrl!).Query);
            var state = queryParams.Get("state");
            var recoveredForm = JsonConvert.DeserializeObject<ServiceAuthStateModel>(state);

            Assert.Equal(form.State, recoveredForm.State);
            Assert.Equal(new Uri(form.RedirectUrl), new Uri(recoveredForm.RedirectUrl));
        }

        [Fact, Priority(10)]
        public async Task SignOutFromParentService()
        {
            var response = await AreaApi.SignOutServiceById(_service.Id);

            AssertExtension.SuccessfulApiResponse(response);
        }
    }
}