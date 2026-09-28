using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Messaging;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Messaging
{
    public class ConversationRepository : BaseRepository, IConversationRepository
    {
        public ConversationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Conversation?> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Conversations
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<PagedList<Conversation>> FindByUser(Guid userId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Conversations
                .Where(c => c.Participants.Any(p => p.UserId == userId));
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(c => c.Participants)
                .OrderByDescending(c => c.UpdatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<Conversation> Save(Conversation conversation, CancellationToken cancellationToken = default)
        {
            if (!Context.Conversations.Contains(conversation)) Context.Conversations.Add(conversation);
            await Context.SaveChangesAsync(cancellationToken);
            return conversation;
        }

        public async Task<Message> SaveMessage(Message message, CancellationToken cancellationToken = default)
        {
            if (!Context.Messages.Contains(message)) Context.Messages.Add(message);
            await Context.SaveChangesAsync(cancellationToken);
            return message;
        }

        public async Task<MessageAttachment> SaveAttachment(MessageAttachment attachment, CancellationToken cancellationToken = default)
        {
            if (!Context.MessageAttachments.Contains(attachment)) Context.MessageAttachments.Add(attachment);
            await Context.SaveChangesAsync(cancellationToken);
            return attachment;
        }

        public async Task<bool> IsParticipant(Guid conversationId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.ConversationParticipants.AnyAsync(p => p.ConversationId == conversationId && p.UserId == userId, cancellationToken);
        }

        public async Task<PagedList<Message>> FindMessages(Guid conversationId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Messages.Where(m => m.ConversationId == conversationId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(m => m.Attachments)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }
    }
}