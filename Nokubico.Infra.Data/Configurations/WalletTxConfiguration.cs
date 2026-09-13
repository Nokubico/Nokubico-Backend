using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    public class WalletTxConfiguration : IEntityTypeConfiguration<WalletTx>
    {
        public void Configure(EntityTypeBuilder<WalletTx> builder)
        {
            builder.ToTable("wallet_txs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Amount).IsRequired();
            builder.HasOne(x => x.Wallet).WithMany(w => w.Transactions).HasForeignKey(x => x.WalletId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }
}
