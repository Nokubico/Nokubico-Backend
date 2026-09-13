using System;
using System.Collections.Generic;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string? Title { get; private set; }
        public string? Slug { get; private set; }
        public string? Description { get; private set; }
        public string? Category { get; private set; }
        public long Price { get; private set; }
        public string Currency { get; private set; } = "USD";
        public string? Thumbnail { get; private set; }
        public string? License { get; private set; }
        public string? DownloadUrl { get; private set; }
        public Guid CreatorId { get; private set; }
        public ProductStatus Status { get; private set; }

        public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();
        public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();

        public void SetTitle(string? title)
        {
            Title = title;
            Touch();
        }

        public void SetPrice(long price)
        {
            Price = price;
            Touch();
        }

        public void SetCurrency(string currency)
        {
            Currency = currency;
            Touch();
        }

        public void AddImage(ProductImage img)
        {
            if (img != null) Images.Add(img);
        }

        public void AddOrderItem(OrderItem item)
        {
            if (item != null) OrderItems.Add(item);
        }

        public void AddReview(Review review)
        {
            if (review != null) Reviews.Add(review);
        }
    }
}
