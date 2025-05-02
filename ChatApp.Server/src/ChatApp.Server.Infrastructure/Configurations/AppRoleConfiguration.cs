using ChatApp.Server.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Server.Infrastructure.Configurations;
internal sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.Metadata.RemoveIndex(new[] { builder.Property(p => p.NormalizedName).Metadata });

        builder.HasIndex(p => new {p.NormalizedName, p.ServerId}).HasDatabaseName("RoleNameServerIdIndex").IsUnique();

        builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(256);

        builder.Property(r => r.NormalizedName)
               .IsRequired()
               .HasMaxLength(256);
    }
}