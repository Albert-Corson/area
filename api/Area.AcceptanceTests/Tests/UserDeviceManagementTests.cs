using System.Linq;
using System.Net;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Fixtures;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Utilities;
using Xunit;
using Xunit.Priority;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(AreaCollection))]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UserDeviceManagementTests : IClassFixture<AuthenticatedTestsFixture>
    {
        private static uint _currentDeviceId;
        private readonly AuthenticatedTestsFixture _authenticatedTestsFixture;

        private AreaApi AreaApi => _authenticatedTestsFixture.AreaApi;

        public UserDeviceManagementTests(AuthenticatedTestsFixture authenticatedTestsFixture)
        {
            _authenticatedTestsFixture = authenticatedTestsFixture;
        }

        [Fact, Priority(1)]
        public async void GetMyDevices()
        {
            var response = await AreaApi.GetMyDevices();

            AssertExtension.SuccessfulApiResponse(response);
            Assert.Single(response.Content.Data!.Devices);
            Assert.Equal(response.Content.Data!.CurrentDevice, response.Content.Data!.Devices.First().Id);

            _currentDeviceId = response.Content.Data!.CurrentDevice;
        }

        [Fact, Priority(2)]
        public async void DeleteMyDevice()
        {
            var response = await AreaApi.DeleteMyDevice(_currentDeviceId);

            AssertExtension.SuccessfulApiResponse(response);
        }

        [Fact, Priority(3)]
        public async void CheckAccessTokenValidity()
        {
            var response = await AreaApi.GetMyDevices();

            AssertExtension.FailedApiResponse(response, HttpStatusCode.Unauthorized);
        }

        [Fact, Priority(4)]
        public async void CheckRefreshTokenValidity()
        {
            var response = await AreaApi.RefreshAccessToken(new RefreshTokenModel {
                RefreshToken = AreaApi.Tokens!.RefreshToken
            });

            AssertExtension.FailedApiResponse(response, HttpStatusCode.Unauthorized);
        }

        [Fact, Priority(5)]
        public async void SignBackIn()
        {
            var response = await AreaApi.SignIn(new SignInModel {
                Identifier = _authenticatedTestsFixture.RegisterForm.Email,
                Password = _authenticatedTestsFixture.RegisterForm.Password
            });

            AssertExtension.SuccessfulApiResponse(response);
        }

        [Fact, Priority(6)]
        public async void CheckOldAccessTokenValidity()
        {
            var response = await AreaApi.GetMyDevices();

            AssertExtension.FailedApiResponse(response, HttpStatusCode.Unauthorized);
        }

        [Fact, Priority(7)]
        public async void CheckOldRefreshTokenValidity()
        {
            var response = await AreaApi.RefreshAccessToken(new RefreshTokenModel {
                RefreshToken = AreaApi.Tokens!.RefreshToken
            });

            AssertExtension.FailedApiResponse(response, HttpStatusCode.Unauthorized);
        }
        
        [Fact, Priority(8)]
        public async void CompareDeviceIds()
        {
            _authenticatedTestsFixture.SignIn();

            var response = await AreaApi.GetMyDevices();

            AssertExtension.SuccessfulApiResponse(response);
            Assert.Equal(_currentDeviceId, response.Content.Data!.CurrentDevice);

        }
    }
}