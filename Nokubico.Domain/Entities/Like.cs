using System;

namespace Nokubico.Domain.Entities
{
    public class Like
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid PostId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public void SetUserAndPost(Guid userId, Guid postId)
        {
            UserId = userId;
            PostId = postId;
        }
    }
}
