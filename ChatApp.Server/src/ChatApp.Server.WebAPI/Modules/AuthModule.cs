using ChatApp.Server.Application.Auth;
using MediatR;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class AuthModule
{
    public static void RegisterAuthRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("login",
            async (ISender sender, LoginCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<LoginCommandResponse>>();

        group.MapPost("register",
           async (ISender sender, RegisterCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .Produces<Result<LoginCommandResponse>>();

        group.MapGet("confirm-email",
           async (ISender sender, [AsParameters]ConfirmEmailCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .Produces<Result<string>>();

        group.MapGet("permissions",
           async (ISender sender, [AsParameters] GetPermissionsQuery request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .Produces<Result<string[]>>();
    }
}
