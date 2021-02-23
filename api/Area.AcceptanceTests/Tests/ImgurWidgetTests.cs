using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private static void CheckParamValue(IEnumerable<ParamModel> parameters, string value = "hot")
        {
            var param = parameters.Single(model => model.Name == "section");
            Assert.Equal(value, param.Value);
        }

        [Fact, Priority(1)]
        public void PreCallParamValueChecks()
        {
            var widget = AreaApi.GetWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);
            var myWidget = AreaApi.GetMyWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);

            Assert.True(widget == myWidget);
            CheckParamValue(widget.Params);
        }

        [Fact, Priority(2)]
        public async void CallWithoutParams()
        {
            var response = await AreaApi.CallWidgetById(IMGUR_GALLERY);

            AssertExtension.SuccessfulApiResponse(response);
            CheckParamValue(response.Content.Data!.CallParams);
        }
        
        [Fact, Priority(3)]
        public void PostCallWithoutParamValueChecks()
        {
            PreCallParamValueChecks();
        }
        
        [Fact, Priority(4)]
        public async void CallWithParams()
        {
            var response = await AreaApi.CallWidgetById(IMGUR_GALLERY, "?section=top");
        
            AssertExtension.SuccessfulApiResponse(response);
            CheckParamValue(response.Content.Data!.CallParams, "top");
            Thread.Sleep(1000);
        }
        
        [Fact, Priority(5)]
        public void PostCallWithParamValueChecks()
        {
            var widget = AreaApi.GetWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);
            var myWidget = AreaApi.GetMyWidgets().Result.Content.Data!
                .Single(model => model.Id == IMGUR_GALLERY);
        
            Assert.True(widget != myWidget);
            CheckParamValue(widget.Params);
            CheckParamValue(myWidget.Params, "top");
        }
    }
}