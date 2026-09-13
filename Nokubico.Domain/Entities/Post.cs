using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string? Content { get; set; }
        public string? Image { get; set; }
        public string? Video { get; set; }
        public Guid AuthorId { get; set; }
        public Guid? SharedPostId { get; set; }

        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
