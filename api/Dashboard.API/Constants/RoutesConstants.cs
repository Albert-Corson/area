namespace Dashboard.API.Constants
{
    public static class RoutesConstants
    {
        public static class Default
        {
            public const string Error = "/Error";
            public const string AboutDotJson = "/About.json";
        }

        public static class Auth
        {
            private const string Base = "/auth";

            public const string SignIn = Base + "/token";
            public const string RefreshAccessToken = Base + "/refresh";
            public const string RevokeUserTokens = Base + "/revoke";
        }

        public static class Users
        {
            private const string Base = "/users";

            public const string SignUp = Base;
            public const string GetMyUser = Base + "/me";
            public const string GetUser = Base + "/{userId}";
            public const string DeleteUser = Base + "/{userId}";
        }

        public static class Services
        {
            private const string Base = "/services";

            public const string GetServices = Base;
            public const string GetMyServices = Base + "/me";
            public const string GetService = Base + "/{serviceId}";
            public const string SignInService = Base + "/{serviceId}";
            public const string SignOutService = Base + "/{serviceId}";
            public const string SignInServiceCallback = Base + "/{serviceId}/callback";
        }

        public static class Widgets
        {
            private const string Base = "/widgets";

            public const string GetWidgets = Base;
            public const string GetMyWidgets = Base + "/me";
            public const string CallWidget = Base + "/{widgetId}";
            public const string SubscribeWidget = Base + "/{widgetId}";
            public const string UnsubscribeWidget = Base + "/{widgetId}";
        }
    }
}
