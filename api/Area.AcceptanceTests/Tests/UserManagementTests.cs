using System.Net;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Utilities;
using Xunit;
using Xunit.Priority;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(AreaCollection))]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UserEndpointTests : IClassFixture<AreaApi>
    {
        private readonly AreaApi _areaApi;
        private readonly RegisterModel _registerForm = new RegisterModel {
            Email = "some@email.adress",
            Password = "Some*Password1234",
            Username = "SomeUsername"
        };

        public UserEndpointTests(AreaApi areaApi)
        {
            _areaApi = areaApi;
        }

        [Fact, Priority(1)]
        public async void Register()
        {
            var response = await _areaApi.Register(_registerForm);

            AssertExtension.SuccessfulApiResponse(response);
        }

        [Fact, Priority(2)]
        public async void SignIn()
        {
            var form = new SignInModel {
                Identifier = _registerForm.Email,
                Password = _registerForm.Password
            };

            var response = await _areaApi.SignIn(form);

            AssertExtension.SuccessfulApiResponse(response);

            _areaApi.Tokens = response.Content.Data;
        }

        [Fact, Priority(3)]
        public async void GetMyUser()
        {
            var response = await _areaApi.GetMyUser();
        
            AssertExtension.SuccessfulApiResponse(response);
            Assert.Equal(_registerForm.Email, response.Content.Data!.Email);
            Assert.Equal(_registerForm.Username, response.Content.Data!.Username);
            Assert.InRange(response.Content.Data!.Id, 1, int.MaxValue);
        }
        
        [Fact, Priority(4)]
        public async void RefreshAccessToken()
        {
            var form = new RefreshTokenModel {
                RefreshToken = _areaApi.Tokens!.RefreshToken
            };
        
            var response = await _areaApi.RefreshAccessToken(form);
        
            AssertExtension.SuccessfulApiResponse(response);
            Assert.NotEqual(_areaApi.Tokens, response.Content.Data);

            _areaApi.Tokens = response.Content.Data;
        }

        [Fact, Priority(5)]
        public async void DeleteMyUser()
        {
            var response = await _areaApi.DeleteMyUser();

            AssertExtension.SuccessfulApiResponse(response);
        }

        [Fact, Priority(6)]
        public async void GetMyDeletedUser()
        {
            var response = await _areaApi.GetMyUser();
        
            AssertExtension.FailedApiResponse(response, HttpStatusCode.Unauthorized);
        }
    }
}