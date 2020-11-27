namespace Dashboard.API.Constants
{
    public static class RoutesConstants
    {
        public static class Auth
        {
            private const string Base = "/auth";

            public const string Login = Base + "/token";
            public const string RefreshAccessToken = Base + "/refresh";
            public const string RevokeAccountTokens = Base + "/revoke";
        }

        public static class Users
        {
            private const string Base = "/users";

            public const string CreateUser = Base;
            public const string UserIdDeleteUser = Base + "/{userId}";
            public const string UserIdGetUser = Base + "/{userId}";
        }

        public static class Services
        {
            private const string Base = "/services";

            public const string GetServices = Base;
            public const string ServiceIdGetService = Base + "/{serviceId}";
            public const string ServiceIdLogin = Base + "/{serviceId}";
            public const string ServiceIdLogout = Base + "/{serviceId}";
        }

        public static class Widgets
        {
            private const string Base = "/widgets";

            public const string GetWidgets = Base;
            public const string WidgetIdGetWidget = "/widgets/{widgetId}";
            public const string WidgetIdSubscribe = "/widgets/{widgetId}";
            public const string WidgetIdUnsubscribe = "/widgets/{widgetId}";
        }
    }
}
