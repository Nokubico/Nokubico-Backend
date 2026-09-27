using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("post");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Author).WithMany(u => u.Posts).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.SharedPost).WithMany().HasForeignKey(x => x.SharedPostId).OnDelete(DeleteBehavior.SetNull);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("now()");
        }
    }
}
