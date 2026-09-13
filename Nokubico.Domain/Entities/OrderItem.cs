using System;

namespace Nokubico.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public long Price { get; private set; }
        public string? Title { get; private set; }
        public string? License { get; private set; }

        public void SetPrice(long price)
        {
            Price = price;
        }

        public void SetTitle(string? title)
        {
            Title = title;
        }
    }
}
