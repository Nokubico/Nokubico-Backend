using System;

namespace Nokubico.Domain.Entities
{
    public class Share : IEntity
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid PostId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User? User { get; private set; }
        public Post? Post { get; private set; }

        public Share()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Share(User user, Post post)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            SetUserAndPost(user, post);
        }

        public void SetUserAndPost(Guid userId, Guid postId)
        {
            UserId = userId;
            PostId = postId;
        }

        public void SetUserAndPost(User user, Post post)
        {
            User = user;
            Post = post;
            UserId = user.Id;
            PostId = post.Id;
        }
    }
}
