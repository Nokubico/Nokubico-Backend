using System;
using System.Collections.Generic;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public long Price { get; set; }
        public string Currency { get; set; } = "USD";
        public string? Thumbnail { get; set; }
        public string? License { get; set; }
        public string? DownloadUrl { get; set; }
        public Guid CreatorId { get; set; }
        public ProductStatus Status { get; set; }

        public ICollection<ProductImage>? Images { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
