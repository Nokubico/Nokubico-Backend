using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("wallets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Balance).IsRequired();
            builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("now()");
        }
    }
}
