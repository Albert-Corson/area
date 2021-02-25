using System;
using Area.AcceptanceTests.Models.Requests;
using Xunit;

namespace Area.AcceptanceTests.Fixtures
{
    public class AuthenticatedTestsFixture : IDisposable
    {
        private static int _counter;

        public readonly AreaApi AreaApi = new AreaApi();

        public readonly RegisterModel RegisterForm = new RegisterModel {
            Email = $"some{_counter}@email.address",
            Password = "Some*Password1234",
            Username = $"SomeUserName_{_counter}"
        };

        public AuthenticatedTestsFixture()
        {
            ++_counter;

            AreaApi.Register(RegisterForm).Wait();

            SignIn();
        }

        public void Dispose()
        {
            AreaApi.DeleteMyUser().Wait();
        }

        public void SignIn()
        {
            var response = AreaApi.SignIn(new SignInModel(RegisterForm)).Result;
            AreaApi.Tokens = response.Content.Data;
        }
    }
}