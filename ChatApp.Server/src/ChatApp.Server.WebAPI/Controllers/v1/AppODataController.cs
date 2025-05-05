using ChatApp.Server.Application.Channels;
using ChatApp.Server.Application.Messages;
using ChatApp.Server.Application.Roles;
using ChatApp.Server.Application.ServerMembers;
using ChatApp.Server.Application.Servers;
using ChatApp.Server.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ChatApp.Server.WebAPI.Controllers.v1;

[Route("odata/v1")]
[ApiController]
[EnableQuery]
public class AppODataController(
    ISender sender) : ODataController
{

    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();

        return builder.GetEdmModel();
    }

    [HttpGet("users")]
    public async Task<IQueryable<UserGetAllQueryResponse>> GetUsers(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetAllQuery(), cancellationToken);
        return response;
    }
}

