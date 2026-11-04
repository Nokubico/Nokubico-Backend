using System;
using System.Collections.Generic;
using Nokubico.Domain.Enums;
using Nokubico.Domain.Validation;

namespace Nokubico.Domain.Entities
{
    public class Product : BaseEntity
    {
        public const int MaximumImages = 10;

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

        public User? Creator { get; private set; }

        protected Product()
        {
        }

        public Product(string title, string? slug, string category, long price, string currency, Guid creatorId)
        {
            Title = title;
            Slug = slug;
            Category = category;
            Price = price;
            Currency = currency;
            CreatorId = creatorId;
            Status = ProductStatus.Draft;
        }

        public void SetCreator(User creator)
        {
            Creator = creator;
            CreatorId = creator.Id;
            Touch();
        }

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

        public void SetDetails(string? slug, string? description, string? category, string? license, string? thumbnail)
        {
            Slug = slug;
            Description = description;
            Category = category;
            License = license;
            Thumbnail = thumbnail;
            Touch();
        }

        public void ReplaceImages(IReadOnlyCollection<string> imageUrls)
        {
            ArgumentNullException.ThrowIfNull(imageUrls);

            if (imageUrls.Count > MaximumImages)
                throw new DomainException($"Um produto pode ter no máximo {MaximumImages} imagens.");

            if (imageUrls.Any(string.IsNullOrWhiteSpace))
                throw new DomainException("Os URLs das imagens não podem estar vazios.");

            if (imageUrls.Distinct(StringComparer.Ordinal).Count() != imageUrls.Count)
                throw new DomainException("Os URLs das imagens não podem repetir-se.");

            Images.Clear();

            var position = 0;
            foreach (var imageUrl in imageUrls)
            {
                Images.Add(new ProductImage(this, imageUrl, checked((short)position)));
                position++;
            }

            Touch();
        }

        public void Publish()
        {
            if (Status != ProductStatus.Draft)
                throw new DomainException("Só é possível publicar um produto em rascunho.");

            Status = ProductStatus.Published;
            Touch();
        }

        public void Disable()
        {
            if (Status != ProductStatus.Published)
                throw new DomainException("Só é possível desativar um produto publicado.");

            Status = ProductStatus.Disabled;
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
