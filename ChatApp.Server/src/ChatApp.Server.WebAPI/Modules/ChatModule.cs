using ChatApp.Server.Application.Channels;
using ChatApp.Server.Application.Chats;
using MediatR;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class ChatModule
{
    public static void RegisterChatRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("chats").WithTags("Chats");

        group.MapPost("/create", async (ISender sender, ChatCreateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();
    }
}
