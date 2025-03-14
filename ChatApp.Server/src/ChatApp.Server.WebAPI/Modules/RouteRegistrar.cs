namespace ChatApp.Server.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterEmployeeRoutes();
        app.RegisterAuthRoutes();
        app.RegisterUserRoutes();
        app.RegisterChannelRoutes();
        app.RegisterChatRoutes();
    }
}
