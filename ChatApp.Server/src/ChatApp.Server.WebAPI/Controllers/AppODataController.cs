using ChatApp.Server.Application.Channels;
using ChatApp.Server.Application.Chats;
using ChatApp.Server.Application.Employees;
using ChatApp.Server.Application.Users;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Chats;
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
        builder.EntitySet<UserGetQueryResponse>("user");
        builder.EntitySet<Channel>("channel");
        builder.EntitySet<Chat>("chat");
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

    [HttpGet("channel/{Id}")]
    public async Task<Result<Channel>> GetChannel(Guid Id, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new ChannelGetQuery(Id), cancellationToken);
        return response;
    }

    [HttpGet("chat/{Id}")]
    public async Task<Result<Chat>> GetChat(Guid Id, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new ChatGetQuery(Id), cancellationToken);
        return response;
    }
}

