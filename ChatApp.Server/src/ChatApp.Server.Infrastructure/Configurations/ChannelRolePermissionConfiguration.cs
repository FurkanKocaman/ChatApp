using ChatApp.Server.Domain.ChannelRolePermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Server.Infrastructure.Configurations;
internal sealed class ChannelRolePermissionConfiguration : IEntityTypeConfiguration<ChannelRolePermission>
{
    public void Configure(EntityTypeBuilder<ChannelRolePermission> builder)
    {
        builder.HasKey(x => new {x.ChannelId, x.RoleId});

        builder.HasOne(x => x.Channel)
              .WithMany(c => c.RolePermissions)
              .HasForeignKey(x => x.ChannelId);

        builder.HasOne(x => x.Role)
               .WithMany()
               .HasForeignKey(x => x.RoleId);
    }
}
