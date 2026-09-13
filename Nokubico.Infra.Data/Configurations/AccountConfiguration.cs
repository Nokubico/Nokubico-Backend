using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AccountId).IsRequired();
            builder.Property(x => x.ProviderId).IsRequired();
            builder.HasIndex(x => new { x.ProviderId, x.AccountId }).IsUnique();
            builder.HasOne(x => x.User).WithMany(u => u.Accounts).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("now()");
        }
    }
}
