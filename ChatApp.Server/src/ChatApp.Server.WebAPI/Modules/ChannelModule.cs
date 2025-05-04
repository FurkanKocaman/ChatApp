using ChatApp.Server.Application.Channels;
using MediatR;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class ChannelModule
{
    public static void RegisterChannelRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/channels").WithTags("Channels");

        group.MapPost("/create",
            async (ISender sender, ChannelCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.CreateChannel).Produces<Result<string>>();

        group.MapPut("/update",
            async (ISender sender, ChannelUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.EditChannel).Produces<Result<string>>();

        group.MapGet("/get",
            async (ISender sender, [AsParameters] ChannelGetQuery request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<ChannelGetQueryResponse>>();
    }
}
