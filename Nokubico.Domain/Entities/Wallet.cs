using System;
using System.Collections.Generic;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public long Balance { get; private set; }
        public string Currency { get; private set; } = "KZ";
        public WalletStatus Status { get; private set; }

        public ICollection<WalletTx> Transactions { get; private set; } = new List<WalletTx>();

        public void AddTransaction(WalletTx tx)
        {
            if (tx != null) Transactions.Add(tx);
        }

        public void SetBalance(long balance)
        {
            Balance = balance;
            Touch();
        }
    }
}
