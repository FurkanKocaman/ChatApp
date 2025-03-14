using ChatApp.Server.Application.Employees;
using ChatApp.Server.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using TS.Result;

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
        builder.EntitySet<EmployeeGetAllQueryResponse>("employees");
        return builder.GetEdmModel();
    }

    [HttpGet("employees")]

    public async Task<IQueryable<EmployeeGetAllQueryResponse>> GetAllEmployees(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new EmployeeGetAllQuery(),cancellationToken);

        return response;
    }
    [HttpGet("user/{Id}")]
    public async Task<Result<UserGetQueryResponse>> GetUser(Guid Id, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetQuery(Id), cancellationToken);
        return response;
    }
}

