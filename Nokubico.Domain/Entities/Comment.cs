using System;

namespace Nokubico.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public string? Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Guid AuthorId { get; private set; }
        public Guid PostId { get; private set; }

        public void SetContent(string? content)
        {
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAuthor(Guid authorId)
        {
            AuthorId = authorId;
        }

        public void SetPost(Guid postId)
        {
            PostId = postId;
        }
    }
}
