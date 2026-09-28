using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Community
{
    public interface IPostRepository
    {
        Task<Post?> FindById(Guid id, CancellationToken cancellationToken = default);

        Task<PagedList<Post>> FindFeed(PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<PagedList<Post>> FindByAuthor(Guid authorId, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<Post> Save(Post post, CancellationToken cancellationToken = default);

        Task Delete(Post post, CancellationToken cancellationToken = default);

        Task<Like?> FindLike(Guid postId, Guid userId, CancellationToken cancellationToken = default);

        Task<Like> SaveLike(Like like, CancellationToken cancellationToken = default);

        Task DeleteLike(Like like, CancellationToken cancellationToken = default);

        Task<Comment> SaveComment(Comment comment, CancellationToken cancellationToken = default);

        Task DeleteComment(Comment comment, CancellationToken cancellationToken = default);

        Task<PagedList<Comment>> FindCommentsByPost(Guid postId, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<Share> SaveShare(Share share, CancellationToken cancellationToken = default);

        Task DeleteShare(Share share, CancellationToken cancellationToken = default);

        Task<bool> IsSharedBy(Guid postId, Guid userId, CancellationToken cancellationToken = default);

        Task<Bookmark> SaveBookmark(Bookmark bookmark, CancellationToken cancellationToken = default);

        Task DeleteBookmark(Bookmark bookmark, CancellationToken cancellationToken = default);

        Task<bool> IsBookmarkedBy(Guid postId, Guid userId, CancellationToken cancellationToken = default);

        Task<long> CountLikes(Guid postId, CancellationToken cancellationToken = default);

        Task<long> CountComments(Guid postId, CancellationToken cancellationToken = default);

        Task<long> CountShares(Guid postId, CancellationToken cancellationToken = default);
    }
}