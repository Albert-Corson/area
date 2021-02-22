using System;
using System.Linq;
using System.Net;
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
    public class WidgetManagementTests : IClassFixture<AuthenticatedTestsFixture>
    {
        private readonly AuthenticatedTestsFixture _authenticatedTestsFixture;
        private static WidgetModel _authed = null!;
        private static WidgetModel _notAuthed = null!;
        private static int _otherServiceId;

        private AreaApi AreaApi => _authenticatedTestsFixture.AreaApi;

        public WidgetManagementTests(AuthenticatedTestsFixture authenticatedTestsFixture)
        {
            _authenticatedTestsFixture = authenticatedTestsFixture;
        }

        [Fact, Priority(1)]
        public async void GetWidgets()
        {
            var response = await AreaApi.GetWidgets();

            AssertExtension.SuccessfulApiResponse(response);

            var rand = new Random();
            var notAuthed = response.Content.Data!.Where(model => !model.RequiresAuth).ToList();
            var authed = response.Content.Data!.Where(model => model.RequiresAuth).ToList();

            _notAuthed = notAuthed.ElementAt(rand.Next(0, notAuthed.Count - 1));
            _authed = authed.ElementAt(rand.Next(0, authed.Count - 1));

            for (var i = 1; i <= 3; i++) {
                if (_authed.Service.Id == i || _notAuthed.Service.Id == i)
                    continue;
                _otherServiceId = i;
                break;
            }
        }

        [Fact, Priority(2)]
        public async void GetWidgetsFromService()
        {
            var notAuthed = _notAuthed;

            var response = await AreaApi.GetWidgets(notAuthed.Service.Id);

            AssertExtension.SuccessfulApiResponse(response);

            foreach (var it in response.Content.Data!) {
                Assert.Equal(notAuthed.Service.Id, it.Service.Id);
            }

            Assert.NotNull(response.Content.Data!.SingleOrDefault(model => model.Id == notAuthed.Id));
        }

        [Fact, Priority(3)]
        public void Subscribe()
        {
            var response1 = AreaApi.SubscribeWidgetById(_notAuthed.Id);
            var response2 = AreaApi.SubscribeWidgetById(_authed.Id);

            AssertExtension.SuccessfulApiResponse(response1.Result);
            AssertExtension.SuccessfulApiResponse(response2.Result);
        }

        [Fact, Priority(4)]
        public async void GetMyServices()
        {
            var response = await AreaApi.GetMyServices();

            AssertExtension.SuccessfulApiResponse(response);
            Assert.NotNull(response.Content.Data!.SingleOrDefault(model => model.Id == _authed.Service.Id));
            Assert.NotNull(response.Content.Data!.SingleOrDefault(model => model.Id == _notAuthed.Service.Id));
        }

        [Fact, Priority(5)]
        public async void GetMyWidgets()
        {
            var response = await AreaApi.GetMyWidgets();

            AssertExtension.SuccessfulApiResponse(response);

            Assert.NotNull(response.Content.Data!.SingleOrDefault(model => model.Id == _notAuthed.Id));
            Assert.NotNull(response.Content.Data!.SingleOrDefault(model => model.Id == _authed.Id));
        }

        [Fact, Priority(6)]
        public async void GetMyWidgetsFromService()
        {
            var response = await AreaApi.GetMyWidgets(_notAuthed.Service.Id);

            AssertExtension.SuccessfulApiResponse(response);

            Assert.Single(response.Content.Data!.Where(model => model.Id == _notAuthed.Id));
        }

        [Fact, Priority(7)]
        public async void GetMyWidgetsFromOtherService()
        {
            var response = await AreaApi.GetMyWidgets(_otherServiceId);

            AssertExtension.SuccessfulApiResponse(response);
            Assert.Empty(response.Content.Data!);
        }

        [Fact, Priority(8)]
        public async void CallAuthedWidgets()
        {
            var response = await AreaApi.CallWidgetById(_authed.Id);

            AssertExtension.FailedApiResponse(response, HttpStatusCode.Unauthorized);
        }

        [Fact, Priority(9)]
        public void Unsubscribe()
        {
            var response1 = AreaApi.UnsubscribeWidgetById(_authed.Id);
            var response2 = AreaApi.UnsubscribeWidgetById(_notAuthed.Id);

            AssertExtension.SuccessfulApiResponse(response1.Result);
            AssertExtension.SuccessfulApiResponse(response2.Result);
        }

        [Fact, Priority(10)]
        public async void GetMyUnsubscribedServices()
        {
            var response = await AreaApi.GetMyServices();

            AssertExtension.SuccessfulApiResponse(response);
            Assert.Empty(response.Content.Data!);
        }

        [Fact, Priority(11)]
        public async void GetMyUnsubscribedWidgets()
        {
            var response = await AreaApi.GetMyWidgets();

            AssertExtension.SuccessfulApiResponse(response);
            Assert.Empty(response.Content.Data!);
        }
    }
}