using System.Net;
using System.Threading.Tasks;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Fixtures;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Utilities;
using Xunit;
using Xunit.Priority;

namespace Area.AcceptanceTests.Tests.Password
{
    [Collection(nameof(AreaCollection))]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ChangePasswordTests : IClassFixture<AuthenticatedTestsFixture>
    {
        private readonly AuthenticatedTestsFixture _authenticatedTestsFixture;
        private const string NewPassword = "S^(*ome*Password1234";
        private AreaApi AreaApi => _authenticatedTestsFixture.AreaApi;

        public ChangePasswordTests(AuthenticatedTestsFixture authenticatedTestsFixture)
        {
            _authenticatedTestsFixture = authenticatedTestsFixture;
        }

        [Fact, Priority(1)]
        public async Task ChangePasswordWithIncorrect()
        {
            ChangePasswordModel form = new ChangePasswordModel
            {
                Old = "incorrect",
                New = NewPassword,
            };
            var response = await AreaApi.ChangePassword(form);

            AssertExtension.FailedApiResponse(response, HttpStatusCode.BadRequest);
        }

        [Fact, Priority(2)]
        public async Task ChangePassword()
        {
            ChangePasswordModel form = new ChangePasswordModel
            {
                Old = _authenticatedTestsFixture.RegisterForm.Password,
                New = NewPassword,
            };
            var response = await AreaApi.ChangePassword(form);

            AssertExtension.SuccessfulApiResponse(response);
        }

        [Fact, Priority(3)]
        public async Task MakeRequestWithOldToken()
        {
            var response = await AreaApi.GetMyUser();

            AssertExtension.SuccessfulApiResponse(response);
        }

        [Fact, Priority(4)]
        public async Task GetTokenWithNewPassword()
        {
            var form = new SignInModel
            {
                Identifier = _authenticatedTestsFixture.RegisterForm.Email,
                Password = NewPassword,
            };

            var response = await AreaApi.SignIn(form);

            AssertExtension.SuccessfulApiResponse(response);

            AreaApi.Tokens = response.Content.Data;
        }
    }
}