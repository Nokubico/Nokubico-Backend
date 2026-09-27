using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public long Total { get; private set; }
        public string Currency { get; private set; } = "USD";
        public string? Status { get; private set; }
        public string? PaymentStatus { get; private set; }
        public string? PaymentMethod { get; private set; }

        public User? User { get; private set; }

        protected Order()
        {
        }

        public Order(Guid userId, string currency)
        {
            UserId = userId;
            Currency = currency;
            Total = 0;
        }

        public void SetUser(User user)
        {
            User = user;
            UserId = user.Id;
            Touch();
        }

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
