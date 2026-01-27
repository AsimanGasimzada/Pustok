using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Core.Entites;

namespace Pustok.DataAccess.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(512);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.Price).IsRequired().HasPrecision(10, 2);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products).HasForeignKey(x => x.CategoryId)
            .HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Restrict);
    }
}
