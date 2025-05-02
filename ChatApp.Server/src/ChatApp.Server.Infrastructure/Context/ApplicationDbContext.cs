using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.ConversationParticipants;
using ChatApp.Server.Domain.Conversations;
using ChatApp.Server.Domain.DirectMessages;
using ChatApp.Server.Domain.FriendShips;
using ChatApp.Server.Domain.Messages;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMemberRoles;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Tokens;
using ChatApp.Server.Domain.UserRoles;
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
    public DbSet<Domain.Servers.Server> Servers { get; set; } = default!;
    public DbSet<Channel> Channels { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<ServerMember> ServerMembers { get; set; } = default!;
    public new DbSet<UserRole> UserRoles { get; set; } = default!;
    public DbSet<FriendShip> FriendShips { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<ConversationParticipant> ConversationParticipants { get; set; } = default!;
    public DbSet<DirectMessage> DirectMessages { get; set; } = default!;
    public DbSet<ServerMemberRole> ServerMemberRoles { get; set; } = default!;
    public DbSet<Token> Tokens { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();

        modelBuilder.Entity<Token>()
            .HasOne(p => p.Server)
            .WithMany()
            .HasForeignKey(p => p.ServerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Token>()
            .HasOne(p => p.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AppRole>()
            .Property(p => p.Level).HasColumnType("decimal(18,2)");      
        
        modelBuilder.Entity<Channel>()
            .HasOne(p => p.Server)
            .WithMany(p => p.Channels)
            .HasForeignKey(p => p.ServerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Message>()
            .HasOne(p => p.Channel)
            .WithMany(p => p.Messages)
            .HasForeignKey(p => p.ChannelId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ServerMember>()
            .HasOne(p => p.Server)
            .WithMany(p => p.Members)
            .HasForeignKey(p => p.ServerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ServerMember>()
            .HasOne(p => p.User)
            .WithMany(p => p.ServerMemberships)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AppRole>()
            .HasOne(p => p.Server)
            .WithMany(p => p.Roles)
            .HasForeignKey(p => p.ServerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Domain.Servers.Server>()
            .HasOne(p => p.Owner)
            .WithMany()
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserRole>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserRole>()
            .HasOne(p => p.Role)
            .WithMany()
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserRole>()
            .HasOne(p => p.Server)
            .WithMany()
            .HasForeignKey(p => p.ServerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ConversationParticipant>()
            .HasOne(p => p.Participant)
            .WithMany(p => p.Conversations)
            .HasForeignKey(p => p.ParticipantId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<ConversationParticipant>()
            .HasOne(p => p.Conversation)
            .WithMany(p => p.Participants)
            .HasForeignKey(p => p.ConversationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<DirectMessage>()
            .HasOne(p => p.Conversation)
            .WithMany()
            .HasForeignKey(p => p.ConversationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<FriendShip>()
            .HasOne(p => p.Requester)
            .WithMany()
            .HasForeignKey(p => p.RequesterId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<FriendShip>()
            .HasOne(p => p.Receiver)
            .WithMany()
            .HasForeignKey(p => p.ReceiverId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ServerMemberRole>()
            .HasOne(p => p.AppRole)
            .WithMany(p => p.ServerMemberRoles)
            .HasForeignKey(p => p.AppRoleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ServerMemberRole>()
            .HasOne(p => p.ServerMember)
            .WithMany(p => p.ServerMemberRoles)
            .HasForeignKey(p => p.ServerMemberId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ServerMemberRole>()
            .HasKey(p => new { p.ServerMemberId, p.AppRoleId });
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();

        if(httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true)
        {
            string userIdString = httpContextAccessor.HttpContext!.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value;
            //string userIdString = "95b12cf8-5095-4b89-bc0d-07e4a826c74c";
            Guid userId = Guid.Parse(userIdString);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.CreateUserId).CurrentValue = userId;
                }
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Property(p => p.IsDeleted).CurrentValue == true)
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
                if (entry.State == EntityState.Deleted)
                {
                    throw new ArgumentException("Cannot delete directly from Db");
                }
            }

        }


        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

