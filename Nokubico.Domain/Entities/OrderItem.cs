using System;

namespace Nokubico.Domain.Entities
{
    public class OrderItem : IEntity
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public long Price { get; private set; }
        public string? Title { get; private set; }
        public string? License { get; private set; }

        public Order? Order { get; private set; }
        public Product? Product { get; private set; }

        public OrderItem()
        {
            Id = Guid.NewGuid();
        }

        public OrderItem(Guid orderId, Guid productId, long price, string? title, string? license)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            ProductId = productId;
            Price = price;
            Title = title;
            License = license;
        }

        public void SetOrder(Order order)
        {
            Order = order;
            OrderId = order.Id;
        }

        public void SetProduct(Product product)
        {
            Product = product;
            ProductId = product.Id;
        }

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
