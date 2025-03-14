using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.ChannelMemberships;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Chats;
using ChatApp.Server.Domain.Messages;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Server.Infrastructure.Context;

internal sealed class ApplicationDbContext: IdentityDbContext<AppUser, AppRole, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Channel> Channels { get; set; } = default!;
    public DbSet<Chat> Chats { get;set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<ChannelMembership> ChannelUsers { get; set; } = default!;



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();

        modelBuilder.Entity<ChannelMembership>()
            .HasKey(p => new { p.ChannelId, p.UserId });

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasKey(p => new { p.UserId, p.RoleId });

    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        string userIdString = httpContextAccessor.HttpContext!.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value;
        //string userIdString = "95b12cf8-5095-4b89-bc0d-07e4a826c74c";
        Guid userId = Guid.Parse(userIdString);

        foreach (var entry in entries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                entry.Property(p => p.CreateUserId).CurrentValue = userId;
            }
            if (entry.State == EntityState.Modified)
            {
                if(entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.DeleteUserId).CurrentValue = userId;
                }
                else
                {
                    entry.Property(p => p.UpdateAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.UpdateUserId).CurrentValue = userId;
                }
               
            }
            if(entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Cannot delete directly from Db");
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

