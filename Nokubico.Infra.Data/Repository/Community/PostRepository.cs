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

        public Post? FindById(Guid id)
        {
            return Context.Posts
                .Include(p => p.Author)
                .Include(p => p.SharedPost)
                .FirstOrDefault(p => p.Id == id);
        }

        public PagedList<Post> FindFeed(PaginationParams pagination)
        {
            var total = Context.Posts.Count();
            var items = Context.Posts
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public PagedList<Post> FindByAuthor(Guid authorId, PaginationParams pagination)
        {
            var query = Context.Posts.Where(p => p.AuthorId == authorId);
            var total = query.Count();
            var items = query
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public Post Save(Post post)
        {
            if (!Context.Posts.Contains(post)) Context.Posts.Add(post);
            Context.SaveChanges();
            return post;
        }

        public void Delete(Post post)
        {
            Context.Posts.Remove(post);
            Context.SaveChanges();
        }

        public Like? FindLike(Guid postId, Guid userId)
        {
            return Context.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);
        }

        public Like SaveLike(Like like)
        {
            if (!Context.Likes.Contains(like)) Context.Likes.Add(like);
            Context.SaveChanges();
            return like;
        }

        public void DeleteLike(Like like)
        {
            Context.Likes.Remove(like);
            Context.SaveChanges();
        }

        public Comment SaveComment(Comment comment)
        {
            if (!Context.Comments.Contains(comment)) Context.Comments.Add(comment);
            Context.SaveChanges();
            return comment;
        }

        public void DeleteComment(Comment comment)
        {
            Context.Comments.Remove(comment);
            Context.SaveChanges();
        }

        public PagedList<Comment> FindCommentsByPost(Guid postId, PaginationParams pagination)
        {
            var query = Context.Comments.Where(c => c.PostId == postId);
            var total = query.Count();
            var items = query
                .Include(c => c.Author)
                .OrderBy(c => c.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public Share SaveShare(Share share)
        {
            if (!Context.Shares.Contains(share)) Context.Shares.Add(share);
            Context.SaveChanges();
            return share;
        }

        public void DeleteShare(Share share)
        {
            Context.Shares.Remove(share);
            Context.SaveChanges();
        }

        public bool IsSharedBy(Guid postId, Guid userId)
        {
            return Context.Shares.Any(s => s.PostId == postId && s.UserId == userId);
        }

        public Bookmark SaveBookmark(Bookmark bookmark)
        {
            if (!Context.Bookmarks.Contains(bookmark)) Context.Bookmarks.Add(bookmark);
            Context.SaveChanges();
            return bookmark;
        }

        public void DeleteBookmark(Bookmark bookmark)
        {
            Context.Bookmarks.Remove(bookmark);
            Context.SaveChanges();
        }

        public bool IsBookmarkedBy(Guid postId, Guid userId)
        {
            return Context.Bookmarks.Any(b => b.PostId == postId && b.UserId == userId);
        }

        public long CountLikes(Guid postId)
        {
            return Context.Likes.Count(l => l.PostId == postId);
        }

        public long CountComments(Guid postId)
        {
            return Context.Comments.Count(c => c.PostId == postId);
        }

        public long CountShares(Guid postId)
        {
            return Context.Shares.Count(s => s.PostId == postId);
        }
    }
}