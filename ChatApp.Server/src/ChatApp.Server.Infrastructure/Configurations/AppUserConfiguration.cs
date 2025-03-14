using ChatApp.Server.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Server.Infrastructure.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(i => i.UserName).IsUnique();
        builder.Property(p => p.FirstName).HasColumnType("varchar(50)");
        builder.Property(P => P.LastName).HasColumnType("varchar(50)");
        builder.Property(P => P.UserName).HasColumnType("varchar(20)");
        builder.Property(P => P.Email).HasColumnType("varchar(MAX)");
    }
}
