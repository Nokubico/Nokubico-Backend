using System;

namespace Nokubico.Domain.Entities
{
    public class Review : IEntity
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid UserId { get; private set; }
        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Product? Product { get; private set; }
        public User? User { get; private set; }

        public Review()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Review(Guid productId, Guid userId, int rating, string? comment)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ProductId = productId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
        }

        public void SetProduct(Product product)
        {
            Product = product;
            ProductId = product.Id;
        }

        public void SetUser(User user)
        {
            User = user;
            UserId = user.Id;
        }

        public void SetRating(int rating)
        {
            Rating = rating;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetComment(string? comment)
        {
            Comment = comment;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
