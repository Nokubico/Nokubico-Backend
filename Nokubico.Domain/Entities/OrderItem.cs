using System;

namespace Nokubico.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public long Price { get; set; }
        public string? Title { get; set; }
        public string? License { get; set; }
    }
}
