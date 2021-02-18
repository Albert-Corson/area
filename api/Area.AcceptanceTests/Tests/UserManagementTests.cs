using System.Net;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Models.Requests;
using Xunit;
using Xunit.Priority;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(ApiTestCollection))]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UserEndpointTests
    {
        private readonly AreaApiClient _areaApiClient;
        private readonly RegisterModel _registerForm = new RegisterModel {
            Email = "some@email.adress",
            Password = "Some*Password1234",
            Username = "SomeUsername"
        };

        public UserEndpointTests(AreaApiClient areaApiClient)
        {
            _areaApiClient = areaApiClient;
        }

        [Fact, Priority(1)]
        public void Register()
        {
            var result = _areaApiClient.Register(_registerForm).Result;

            Assert.Equal(HttpStatusCode.OK, result.Status);
            Assert.Null(result.Data.Error);
            Assert.True(result.Data.Successful);
        }

        [Fact, Priority(2)]
        public void SignIn()
        {
            var form = new SignInModel {
                Identifier = _registerForm.Email,
                Password = _registerForm.Password
            };

            var result = _areaApiClient.SignIn(form).Result;

            Assert.Equal(HttpStatusCode.OK, result.Status);
            Assert.NotNull(result.Data.Data);
            Assert.Null(result.Data.Error);
            Assert.True(result.Data.Successful);

            _areaApiClient.Tokens = result.Data.Data;
        }

        [Fact, Priority(3)]
        public void DeleteAccount()
        {
            var result = _areaApiClient.DeleteMyUser().Result;

            Assert.Equal(HttpStatusCode.OK, result.Status);
            Assert.Null(result.Data.Error);
            Assert.True(result.Data.Successful);
        }

        [Fact, Priority(4)]
        public void GetMyDeletedUser()
        {
            var result = _areaApiClient.GetMyUser().Result;

            Assert.Equal(HttpStatusCode.Unauthorized, result.Status);
            Assert.Null(result.Data.Data);
            Assert.NotNull(result.Data.Error);
            Assert.False(result.Data.Successful);
        }
    }
}