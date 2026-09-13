using System;

namespace Nokubico.Domain.Entities
{
    public class WalletTx
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string Type { get; set; } = null!;
        public long Amount { get; set; }
        public long BalanceBefore { get; set; }
        public DateTime CreatedAt { get; set; }

        public Wallet? Wallet { get; set; }
    }
}
