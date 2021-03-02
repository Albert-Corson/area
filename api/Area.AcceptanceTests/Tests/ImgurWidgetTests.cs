using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Fixtures;
using Area.AcceptanceTests.Models.Responses;
using Area.AcceptanceTests.Utilities;
using Xunit;
using Xunit.Priority;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(AreaCollection))]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ImgurWidgetTests : IClassFixture<AuthenticatedTestsFixture>
    {
        private const int IMGUR_GALLERY = 1;
        private readonly AuthenticatedTestsFixture _authenticatedTestsFixture;

        private AreaApi AreaApi => _authenticatedTestsFixture.AreaApi;

        public ImgurWidgetTests(AuthenticatedTestsFixture authenticatedTestsFixture)
        {
            _authenticatedTestsFixture = authenticatedTestsFixture;

            AreaApi.SubscribeWidgetById(IMGUR_GALLERY).Wait();
        }

        private static void CheckParam(IEnumerable<ParamModel> parameters, string value = "Hot")
        {
            var param = parameters.Single(model => model.Name == "section");
            Assert.Equal(value, param.Value);
        }

        [Fact, Priority(1)]
        public void PreCallParamsChecks()
        {
            var widget = AreaApi.GetWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);
            var myWidget = AreaApi.GetMyWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);

            Assert.True(widget == myWidget);
            CheckParam(widget.Params);
        }

        [Fact, Priority(2)]
        public async Task CallWithoutParams()
        {
            var response = await AreaApi.CallWidgetById(IMGUR_GALLERY);

            AssertExtension.SuccessfulApiResponse(response);
            CheckParam(response.Content.Data!.CallParams);
        }
        
        [Fact, Priority(3)]
        public void PostCallWithoutParamsChecks()
        {
            PreCallParamsChecks();
        }

        [Fact, Priority(4)]
        public async Task CallWithParams()
        {
            var response = await AreaApi.CallWidgetById(IMGUR_GALLERY, "?section=Top");
        
            AssertExtension.SuccessfulApiResponse(response);
            CheckParam(response.Content.Data!.CallParams, "Top");
        }

        [Fact, Priority(5)]
        public void PostCallWithParamsChecks()
        {
            var widget = AreaApi.GetWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);
            var myWidget = AreaApi.GetMyWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);
        
            Assert.True(widget == myWidget);
            CheckParam(widget.Params);
            CheckParam(myWidget.Params, "Top");
        }

        [Fact, Priority(6)]
        public async Task AnotherCallWithoutParams()
        {
            var response = await AreaApi.CallWidgetById(IMGUR_GALLERY);

            AssertExtension.SuccessfulApiResponse(response);
            CheckParam(response.Content.Data!.CallParams, "Top");
        }

        [Fact, Priority(5)]
        public void PostAnotherCallWithoutParamsChecks()
        {
            PostCallWithParamsChecks();
        }
    }
}