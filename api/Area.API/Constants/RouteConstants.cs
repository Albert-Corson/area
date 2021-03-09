namespace Area.API.Constants
{
    public static class RouteConstants
    {
        private const string Api = "/api";

        public const string Docs = "docs";  
        public const string Error = "/error";
        public const string AboutDotJson = Api + "/about";

        public static class Auth
        {
            private const string Root = Api + "/auth";

            public const string SignIn = Root + "/token";
            public const string RefreshAccessToken = Root + "/refresh";
            public const string ExchangeCode = Root + "/code";
            public const string SignInWithFacebook = Root + "/facebook";
            public const string SignInWithFacebookCallback = SignInWithFacebook + "/callback";
            public const string SignInWithGoogle = Root + "/google";
            public const string SignInWithGoogleCallback = SignInWithGoogle + "/callback";
            public const string SignInWithMicrosoft = Root + "/microsoft";
            public const string SignInWithMicrosoftCallback = SignInWithMicrosoft + "/callback";
        }

        public static class Users
        {
            private const string Root = Api + "/users";

            public const string Register = Root;
            public const string GetMyUser = Root + "/me";
            public const string DeleteMyUser = Root + "/me";
            public const string GetMyDevices = Root + "/me/devices";
            public const string DeleteMyDevice = Root + "/me/devices/{deviceId}";
        }

        public static class Services
        {
            private const string Root = Api + "/services";

            public const string GetServices = Root;
            public const string GetMyServices = Root + "/me";
            public const string GetService = Root + "/{serviceId}";
            public const string SignInService = Root + "/{serviceId}/auth";
            public const string SignOutService = Root + "/{serviceId}";
            public const string SignInServiceCallback = Root + "/{serviceId}/callback";
        }

        public static class Widgets
        {
            private const string Root = Api + "/widgets";

            public const string GetWidgets = Root;
            public const string GetMyWidgets = Root + "/me";
            public const string CallWidget = Root + "/{widgetId}";
            public const string SubscribeWidget = Root + "/{widgetId}";
            public const string UnsubscribeWidget = Root + "/{widgetId}";
        }
    }
}