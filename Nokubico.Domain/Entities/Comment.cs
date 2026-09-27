using System;

namespace Nokubico.Domain.Entities
{
    public class Comment : IEntity
    {
        public Guid Id { get; private set; }
        public string? Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Guid AuthorId { get; private set; }
        public Guid PostId { get; private set; }

        public User? Author { get; private set; }
        public Post? Post { get; private set; }

        public Comment()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Comment(Guid authorId, Guid postId, string? content)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            AuthorId = authorId;
            PostId = postId;
            Content = content;
        }

        public void SetAuthor(User author)
        {
            Author = author;
            AuthorId = author.Id;
        }

        public void SetPost(Post post)
        {
            Post = post;
            PostId = post.Id;
        }

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
