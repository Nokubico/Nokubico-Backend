using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public long Total { get; set; }
        public string Currency { get; set; } = "USD";
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<OrderItem>? Items { get; set; }
    }
}
