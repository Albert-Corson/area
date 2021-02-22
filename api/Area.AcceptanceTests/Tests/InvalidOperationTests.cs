using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Area.AcceptanceTests.Constants;
using Area.AcceptanceTests.Fixtures;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Models.Responses;
using Area.AcceptanceTests.Utilities;
using Xunit;

namespace Area.AcceptanceTests.Tests
{
    public class InvalidOperationTests : IClassFixture<AuthenticatedTestsFixture>
    {
        private readonly AuthenticatedTestsFixture _authenticatedTestsFixture;

        private AreaApi AreaApi => _authenticatedTestsFixture.AreaApi;

        public InvalidOperationTests(AuthenticatedTestsFixture authenticatedTestsFixture)
        {
            _authenticatedTestsFixture = authenticatedTestsFixture;
        }

        [Fact]
        public async void InvalidPostForm()
        {
            var result = await AreaApi.Client.PostAsync<ResponseModel<TokensModel>>(RouteConstants.Auth.SignIn);

            AssertExtension.FailedApiResponse(result, HttpStatusCode.BadRequest);
        }

        [Fact]
        public void InvalidRegister()
        {
            var registerForm = new RegisterModel {
                Email = "some-other@email.adress",
                Password = "Some*Password1234",
                Username = "SomeOtherUserName_1234"
            };
            
            var request1 = AreaApi.Register(new RegisterModel(registerForm) {
                Email = "notAnEmail"
            });
            var request2 = AreaApi.Register(new RegisterModel(registerForm) {
                Password = "weakpassword"
            });
            var request3 = AreaApi.Register(new RegisterModel(registerForm) {
                Username = "!!!!!"
            });
            var request4 = AreaApi.Register(new RegisterModel(_authenticatedTestsFixture.RegisterForm));

            AssertExtension.FailedApiResponse(request1.Result, HttpStatusCode.BadRequest);
            AssertExtension.FailedApiResponse(request2.Result, HttpStatusCode.BadRequest);
            AssertExtension.FailedApiResponse(request3.Result, HttpStatusCode.BadRequest);
            AssertExtension.FailedApiResponse(request4.Result, HttpStatusCode.Conflict);
        }

        [Fact]
        public void NoFoundRoute()
        {
            var request1 = AreaApi.Client.GetAsync(RouteConstants.InvalidRoute);
            var request3 = AreaApi.Client.DeleteAsync(RouteConstants.InvalidRoute);
            var request2 = AreaApi.Client.PostAsync(RouteConstants.InvalidRoute);

            AssertExtension.FailedApiResponse(request1.Result, HttpStatusCode.NotFound);
            AssertExtension.FailedApiResponse(request2.Result, HttpStatusCode.NotFound);
            AssertExtension.FailedApiResponse(request3.Result, HttpStatusCode.NotFound);
        }

        [Fact]
        public void InvalidQueryParameterFormat()
        {
            var badRequest1 = AreaApi.Client.GetAsync<ResponseModel<IEnumerable<WidgetModel>>>(RouteConstants.Widgets.GetWidgets + "?serviceId=aaaa");
            var badRequest2 = AreaApi.Client.GetAsync<ResponseModel<IEnumerable<WidgetModel>>>(RouteConstants.Widgets.GetMyWidgets + "?serviceId=aaaa");

            AssertExtension.FailedApiResponse(badRequest1.Result, HttpStatusCode.BadRequest);
            AssertExtension.FailedApiResponse(badRequest2.Result, HttpStatusCode.BadRequest);
        }

        [Fact]
        public void InvalidQueryParameterValue()
        {
            var notFound1 = AreaApi.Client.GetAsync<ResponseModel<IEnumerable<WidgetModel>>>(RouteConstants.Widgets.GetWidgets + "?serviceId=100");
            var notFound2 = AreaApi.Client.GetAsync<ResponseModel<IEnumerable<WidgetModel>>>(RouteConstants.Widgets.GetMyWidgets + "?serviceId=100");
            var notFound3 = AreaApi.GetServiceById(int.MaxValue - 1);

            AssertExtension.FailedApiResponse(notFound1.Result, HttpStatusCode.NotFound);
            AssertExtension.FailedApiResponse(notFound2.Result, HttpStatusCode.NotFound);
            AssertExtension.FailedApiResponse(notFound3.Result, HttpStatusCode.NotFound);
        }
    }
}