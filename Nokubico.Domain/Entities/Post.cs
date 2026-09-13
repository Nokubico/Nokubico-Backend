using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string? Content { get; private set; }
        public string? Image { get; private set; }
        public string? Video { get; private set; }
        public Guid AuthorId { get; private set; }
        public Guid? SharedPostId { get; private set; }

        public ICollection<Like> Likes { get; private set; } = new List<Like>();
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

        public void SetContent(string? content)
        {
            Content = content;
            Touch();
        }

        public void SetImage(string? image)
        {
            Image = image;
            Touch();
        }

        public void SetVideo(string? video)
        {
            Video = video;
            Touch();
        }

        public void AddLike(Like like)
        {
            if (like != null) Likes.Add(like);
        }

        public void AddComment(Comment comment)
        {
            if (comment != null) Comments.Add(comment);
        }
    }
}
