namespace ChatApp.Server.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    { 
        app.RegisterAuthRoutes();
        app.RegisterUserRoutes();
        app.RegisterServerRoutes();
        app.RegisterChannelRoutes();
        app.RegisterMessageRoutes();
    }
}
