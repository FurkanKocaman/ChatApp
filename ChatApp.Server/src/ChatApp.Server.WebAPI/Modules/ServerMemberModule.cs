using ChatApp.Server.Application.ServerMembers;
using MediatR;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class ServerMemberModule
{
    public static void RegisterServerMemberRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/server-members").WithTags("ServerMembers");

        group.MapPost("/join-by-token",
            async (ISender sender, JoinServerByTokenCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
