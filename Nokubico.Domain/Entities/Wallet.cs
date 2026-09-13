using System;
using System.Collections.Generic;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; set; }
        public long Balance { get; set; }
        public string Currency { get; set; } = "USD";
        public WalletStatus Status { get; set; }

        public ICollection<WalletTx>? Transactions { get; set; }
    }
}
