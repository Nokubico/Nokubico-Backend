using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Community
{
    public interface IPostRepository
    {
        Post? FindById(Guid id);

        PagedList<Post> FindFeed(PaginationParams pagination);

        PagedList<Post> FindByAuthor(Guid authorId, PaginationParams pagination);

        Post Save(Post post);

        void Delete(Post post);

        Like? FindLike(Guid postId, Guid userId);

        Like SaveLike(Like like);

        void DeleteLike(Like like);

        Comment SaveComment(Comment comment);

        void DeleteComment(Comment comment);

        PagedList<Comment> FindCommentsByPost(Guid postId, PaginationParams pagination);

        Share SaveShare(Share share);

        void DeleteShare(Share share);

        bool IsSharedBy(Guid postId, Guid userId);

        Bookmark SaveBookmark(Bookmark bookmark);

        void DeleteBookmark(Bookmark bookmark);

        bool IsBookmarkedBy(Guid postId, Guid userId);

        long CountLikes(Guid postId);

        long CountComments(Guid postId);

        long CountShares(Guid postId);
    }
}