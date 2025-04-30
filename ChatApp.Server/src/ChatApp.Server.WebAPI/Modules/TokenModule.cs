using ChatApp.Server.Application.Tokens;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class TokenModule
{
    public static void RegisterTokenRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/token").WithTags("Tokens");

        group.MapPost("/create",
            async (ISender sender, GenerateServerInvitationTokenCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.CreateInvite).Produces<Result<string>>();

        group.MapGet("/validate", async (ISender sender, [FromQuery] string token, CancellationToken cancellationToken) =>
        {
            ValidateServerInvitationTokenCommand request = new(token);
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        })
            .Produces<Result<InviteValidationResponse>>();
    }
}
