using ChatApp.Server.Application.Messages;
using MediatR;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class MessageModule
{
    public static void RegisterMessageRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/messages").WithTags("Messages");

        group.MapPost("/create",
            async (ISender sender, MessageSendCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
