using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Core.Entites;

namespace Pustok.DataAccess.Configurations;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.Fullname).IsRequired().HasMaxLength(256);
    }
}
