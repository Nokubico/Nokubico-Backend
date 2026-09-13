using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public long Total { get; private set; }
        public string Currency { get; private set; } = "USD";
        public string? Status { get; private set; }
        public string? PaymentStatus { get; private set; }
        public string? PaymentMethod { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        public void AddItem(OrderItem item)
        {
            if (item != null) Items.Add(item);
        }

        public void SetTotal(long total)
        {
            Total = total;
            Touch();
        }
    }
}
