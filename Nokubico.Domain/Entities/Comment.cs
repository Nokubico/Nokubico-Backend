using System;

namespace Nokubico.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PostId { get; set; }
    }
}
