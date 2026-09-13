using System;

namespace Nokubico.Domain.Entities
{
    public class WalletTx
    {
        public Guid Id { get; private set; }
        public Guid WalletId { get; private set; }
        public string Type { get; private set; } = null!;
        public long Amount { get; private set; }
        public long BalanceBefore { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Wallet? Wallet { get; private set; }

        public void SetWallet(Wallet wallet)
        {
            Wallet = wallet;
            WalletId = wallet.Id;
        }

        public void SetAmount(long amount)
        {
            Amount = amount;
        }
    }
}
