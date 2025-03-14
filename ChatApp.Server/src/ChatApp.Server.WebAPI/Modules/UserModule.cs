using ChatApp.Server.Application.Users;
using MediatR;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/users").WithTags("Users");

        group.MapPost("/create",
            async (ISender sender, UserCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
