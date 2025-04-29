using ChatApp.Server.Application;
using ChatApp.Server.Infrastructure;
using ChatApp.Server.WebAPI.Controllers;
using ChatApp.Server.WebAPI.Modules;
using ChatApp.Server.WebAPI;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;
using ChatApp.Server.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication;
using PersonelYonetim.Server.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

builder.Services.AddSignalR();

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddOData(opt =>
        opt
        .Select()
        .Filter()
        .Count()
        .Expand()
        .OrderBy()
        .SetMaxTop(null)
        .AddRouteComponents("odata", AppODataController.GetEdmModel()))
    ;
builder.Services.AddRateLimiter(x=>
x.AddFixedWindowLimiter("fixed",cfg =>
{
    cfg.QueueLimit = 100;
    cfg.Window = TimeSpan.FromSeconds(1);
    cfg.PermitLimit = 100;
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();
builder.Services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseCors(x=> x.AllowAnyHeader().AllowCredentials().AllowAnyMethod().SetIsOriginAllowed(t=>true).SetPreflightMaxAge(TimeSpan.FromMinutes(10)));

app.RegisterRoutes();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

ExtensionsMiddleware.CreateFirstUser(app);

app.MapHub<ChatHub>("/chat-hub");

app.Run();
