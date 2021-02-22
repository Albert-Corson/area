using System;
using System.Linq;
using System.Net;
using Area.AcceptanceTests.Fixtures;
using Area.AcceptanceTests.Models.Responses;
using Area.AcceptanceTests.Utilities;
using Xunit;
using Xunit.Priority;

namespace Area.AcceptanceTests.Tests
{
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
        public async void GetServices()
        {
            var response = await AreaApi.GetServices();

            AssertExtension.SuccessfulApiResponse(response);

            var rand = new Random();
            var service = response.Content.Data!.ElementAt(rand.Next(0, response.Content.Data!.Count() - 1));
            _service.Copy(service);
        }

        [Fact, Priority(2)]
        public async void GetServiceById()
        {
            var response = await AreaApi.GetServiceById(_service.Id);

            AssertExtension.SuccessfulApiResponse(response);
            Assert.Equal(_service.Id, response.Content.Data!.Id);
            Assert.Equal(_service.Name, response.Content.Data!.Name);
        }

        [Fact, Priority(3)]
        public async void SignInServiceById()
        {
            var response = await AreaApi.SignInServiceById(_service.Id);

            Assert.True(response.Content.Successful);
            Assert.Null(response.Content.Error);
            Assert.True(response.Status == HttpStatusCode.Accepted || response.Status == HttpStatusCode.OK);
        }

        [Fact, Priority(10)]
        public async void SignOutFromParentService()
        {
            var response = await AreaApi.SignOutServiceById(_service.Id);

            AssertExtension.SuccessfulApiResponse(response);
        }
    }
}