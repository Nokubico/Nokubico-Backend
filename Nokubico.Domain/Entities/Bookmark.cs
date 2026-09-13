using System;

namespace Nokubico.Domain.Entities
{
    public class Bookmark
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
