using Microsoft.AspNetCore.Http;

namespace Area.AcceptanceTests.Constants
{
    public static class RouteConstants
    {
        private const string Api = "/api";

        public const string AboutDotJson = Api + "/about";
        public const string InvalidRoute = "/this/route/leads/to/nowhere";

        public static class Auth
        {
            private const string Root = Api + "/auth";

            public const string SignIn = Root + "/token";
            public const string RefreshAccessToken = Root + "/refresh";
            public const string RevokeUserTokens = Root + "/revoke";
        }

        public static class Users
        {
            private const string Root = Api + "/users";

            public const string Register = Root;
            public const string GetMyUser = Root + "/me";
            public const string DeleteMyUser = Root + "/me";
        }

        public static class Services
        {
            private const string Root = Api + "/services";

            public const string GetServices = Root;
            public const string GetMyServices = Root + "/me";
            public static string GetServiceById(int id) => Root + "/" + id;
            public static string SignInServiceById(int id) => Root + "/" + id;
            public static string SignOutServiceById(int id) => Root + "/" + id;
        }

        public static class Widgets
        {
            private const string Root = Api + "/widgets";

            public const string GetWidgets = Root;
            public const string GetMyWidgets = Root + "/me";
            public static string CallWidgetById(int id) => Root + "/" + id;
            public static string CallWidgetById(int id, QueryString queryString) => CallWidgetById(id) + queryString;
            public static string SubscribeWidgetById(int id) => Root + "/" + id;
            public static string UnsubscribeWidgetById(int id) => Root + "/" + id;
        }
    }
}