using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("session");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Token).IsRequired();
            builder.HasIndex(x => x.Token).IsUnique();
            builder.HasOne(x => x.User).WithMany(u => u.Sessions).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }
}
