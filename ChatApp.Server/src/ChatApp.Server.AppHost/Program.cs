var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ChatApp_Server_WebAPI>("chatapp.server-webapi");

builder.Build().Run();
