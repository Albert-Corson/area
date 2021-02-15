namespace Area.API.Constants
{
    public static class RoutesConstants
    {
        private const string Api = "/api";

        public const string Docs = "docs";  
        public const string Error = "/error";
        public const string AboutDotJson = Api + "/about";

        public static class Auth
        {
            private const string Base = Api + "/auth";

            public const string SignIn = Base + "/token";
            public const string RefreshAccessToken = Base + "/refresh";
            public const string RevokeUserTokens = Base + "/revoke";
        }

        public static class Users
        {
            private const string Base = Api + "/users";

            public const string Register = Base;
            public const string GetMyUser = Base + "/me";
            public const string DeleteMyUser = Base + "/me";
        }

        public static class Services
        {
            private const string Base = Api + "/services";

            public const string GetServices = Base;
            public const string GetMyServices = Base + "/me";
            public const string GetService = Base + "/{serviceId}";
            public const string SignInService = Base + "/{serviceId}";
            public const string SignOutService = Base + "/{serviceId}";
            public const string SignInServiceCallback = Base + "/{serviceId}/callback";
        }

        public static class Widgets
        {
            private const string Base = Api + "/widgets";

            public const string GetWidgets = Base;
            public const string GetMyWidgets = Base + "/me";
            public const string CallWidget = Base + "/{widgetId}";
            public const string SubscribeWidget = Base + "/{widgetId}";
            public const string UnsubscribeWidget = Base + "/{widgetId}";
        }
    }
}