using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.Users;
using ChatApp.Server.Infrastructure.Configurations;
using ChatApp.Server.Infrastructure.Context;
using ChatApp.Server.Infrastructure.Options;
using ChatApp.Server.Infrastructure.Services;
using ChatApp.Server.Infrastructure.SignalR;
using GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PersonelYonetim.Server.Domain.RoleClaim;
using Scrutor;
using StackExchange.Redis;

namespace ChatApp.Server.Infrastructure;

public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("SqlServer")!;
            opt.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IChatHubService, ChatHubService>();
        services.AddScoped<IPermissionCacheService,  PermissionCacheService>();

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:ConnectionString"];
            options.InstanceName = "ChatApp_";
        });

        services.AddHealthChecks()
         .AddCheck<RedisHealthCheck>(
             name: "redis",
             failureStatus: HealthStatus.Unhealthy,
             tags: new[] { "database" });

        services.AddScoped<IRoleValidator<AppRole>, AppRoleValidator>();

        services
            .AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 1;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddAuthorization(options =>
        {
            foreach (var permission in typeof(Permissions).GetFields().Select(f => f.GetValue(null)?.ToString()))
            {
                if (permission is not null)
                {
                    options.AddPolicy(permission, policy =>
                        policy.RequireClaim("permission", permission));
                }
            }
        });

        services.Scan(opt => opt
        .FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
        .AddClasses(publicOnly:false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
        );
        return services;
    }
}

