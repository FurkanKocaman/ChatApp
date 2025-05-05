using ChatApp.Server.Application.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace ChatApp.Server.WebAPI.Modules.v1;

public static class RoleModule
{
    public static void RegisterRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("api/v1/roles").WithTags("Roles");

        group.MapPost("/create",
           async (ISender sender, RoleCreateCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization(Permissions.CreateRole).Produces<Result<string>>();
        group.MapPut("/update",
           async (ISender sender, RoleUpdateCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization(Permissions.EditRole).Produces<Result<string>>();

        group.MapGet("/get",
            async (ISender sender,[FromQuery] Guid serverId, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new RoleGetAllByServerQuery(serverId), cancellationToken);
                return Results.Ok(response) ;
            })
            .RequireAuthorization().Produces<Result<string>>();
    }
}
