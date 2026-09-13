using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_image");
            builder.HasKey(x => new { x.ProductId, x.ImageUrl });
            builder.Property(x => x.Position).IsRequired();
            builder.HasOne(x => x.Product).WithMany(p => p.Images).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
