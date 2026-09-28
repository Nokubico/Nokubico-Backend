using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Community;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Community
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Post?> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Posts
                .Include(p => p.Author)
                .Include(p => p.SharedPost)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PagedList<Post>> FindFeed(PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var total = await Context.Posts.CountAsync(cancellationToken);
            var items = await Context.Posts
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<PagedList<Post>> FindByAuthor(Guid authorId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Posts.Where(p => p.AuthorId == authorId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<Post> Save(Post post, CancellationToken cancellationToken = default)
        {
            if (!Context.Posts.Contains(post)) Context.Posts.Add(post);
            await Context.SaveChangesAsync(cancellationToken);
            return post;
        }

        public async Task Delete(Post post, CancellationToken cancellationToken = default)
        {
            Context.Posts.Remove(post);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Like?> FindLike(Guid postId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId, cancellationToken);
        }

        public async Task<Like> SaveLike(Like like, CancellationToken cancellationToken = default)
        {
            if (!Context.Likes.Contains(like)) Context.Likes.Add(like);
            await Context.SaveChangesAsync(cancellationToken);
            return like;
        }

        public async Task DeleteLike(Like like, CancellationToken cancellationToken = default)
        {
            Context.Likes.Remove(like);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Comment> SaveComment(Comment comment, CancellationToken cancellationToken = default)
        {
            if (!Context.Comments.Contains(comment)) Context.Comments.Add(comment);
            await Context.SaveChangesAsync(cancellationToken);
            return comment;
        }

        public async Task DeleteComment(Comment comment, CancellationToken cancellationToken = default)
        {
            Context.Comments.Remove(comment);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedList<Comment>> FindCommentsByPost(Guid postId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Comments.Where(c => c.PostId == postId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(c => c.Author)
                .OrderBy(c => c.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<Share> SaveShare(Share share, CancellationToken cancellationToken = default)
        {
            if (!Context.Shares.Contains(share)) Context.Shares.Add(share);
            await Context.SaveChangesAsync(cancellationToken);
            return share;
        }

        public async Task DeleteShare(Share share, CancellationToken cancellationToken = default)
        {
            Context.Shares.Remove(share);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsSharedBy(Guid postId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Shares.AnyAsync(s => s.PostId == postId && s.UserId == userId, cancellationToken);
        }

        public async Task<Bookmark> SaveBookmark(Bookmark bookmark, CancellationToken cancellationToken = default)
        {
            if (!Context.Bookmarks.Contains(bookmark)) Context.Bookmarks.Add(bookmark);
            await Context.SaveChangesAsync(cancellationToken);
            return bookmark;
        }

        public async Task DeleteBookmark(Bookmark bookmark, CancellationToken cancellationToken = default)
        {
            Context.Bookmarks.Remove(bookmark);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsBookmarkedBy(Guid postId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Bookmarks.AnyAsync(b => b.PostId == postId && b.UserId == userId, cancellationToken);
        }

        public async Task<long> CountLikes(Guid postId, CancellationToken cancellationToken = default)
        {
            return await Context.Likes.CountAsync(l => l.PostId == postId, cancellationToken);
        }

        public async Task<long> CountComments(Guid postId, CancellationToken cancellationToken = default)
        {
            return await Context.Comments.CountAsync(c => c.PostId == postId, cancellationToken);
        }

        public async Task<long> CountShares(Guid postId, CancellationToken cancellationToken = default)
        {
            return await Context.Shares.CountAsync(s => s.PostId == postId, cancellationToken);
        }
    }
}