using ChatApp.Server.Domain.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Server.Infrastructure.Configurations;
internal sealed class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Description)
               .HasMaxLength(500);

        builder.HasOne(c => c.Server)
               .WithMany(s => s.Channels)
               .HasForeignKey(c => c.ServerId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.Messages)
               .WithOne(m => m.Channel)
               .HasForeignKey(m => m.ChannelId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.RolePermissions)
               .WithOne(rp => rp.Channel)
               .HasForeignKey(rp => rp.ChannelId);
    }
}
