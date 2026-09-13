using System;

namespace Nokubico.Domain.Entities
{
    public class ProductImage
    {
        public Guid ProductId { get; private set; }
        public string ImageUrl { get; private set; } = null!;
        public short Position { get; private set; }

        public Product? Product { get; private set; }

        public void SetImageUrl(string url)
        {
            ImageUrl = url;
        }

        public void SetPosition(short pos)
        {
            Position = pos;
        }

        public void SetProduct(Product product)
        {
            Product = product;
            ProductId = product.Id;
        }
    }
}
