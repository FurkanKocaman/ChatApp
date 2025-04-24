using ChatApp.Server.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ChatApp.Server.WebAPI.Controllers;

[Route("odata")]
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

