using ChatApp.Server.Application.Servers;
using MediatR;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class ServerModule
{
    public static void RegisterServerRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/servers").WithTags("Servers");

        group.MapPost("/create",
            async (ISender sender, ServerCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
