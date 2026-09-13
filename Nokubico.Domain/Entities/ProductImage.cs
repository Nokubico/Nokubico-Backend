using System;

namespace Nokubico.Domain.Entities
{
    public class ProductImage
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public short Position { get; set; }

        public Product? Product { get; set; }
    }
}
