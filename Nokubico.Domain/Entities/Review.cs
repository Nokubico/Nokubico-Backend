using System;

namespace Nokubico.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid UserId { get; private set; }
        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

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
