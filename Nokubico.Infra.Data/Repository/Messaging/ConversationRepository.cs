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

        public Conversation? FindById(Guid id)
        {
            return Context.Conversations
                .Include(c => c.Participants)
                .FirstOrDefault(c => c.Id == id);
        }

        public PagedList<Conversation> FindByUser(Guid userId, PaginationParams pagination)
        {
            var query = Context.Conversations
                .Where(c => c.Participants.Any(p => p.UserId == userId));
            var total = query.Count();
            var items = query
                .Include(c => c.Participants)
                .OrderByDescending(c => c.UpdatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public Conversation Save(Conversation conversation)
        {
            if (!Context.Conversations.Contains(conversation)) Context.Conversations.Add(conversation);
            Context.SaveChanges();
            return conversation;
        }

        public Message SaveMessage(Message message)
        {
            if (!Context.Messages.Contains(message)) Context.Messages.Add(message);
            Context.SaveChanges();
            return message;
        }

        public MessageAttachment SaveAttachment(MessageAttachment attachment)
        {
            if (!Context.MessageAttachments.Contains(attachment)) Context.MessageAttachments.Add(attachment);
            Context.SaveChanges();
            return attachment;
        }

        public bool IsParticipant(Guid conversationId, Guid userId)
        {
            return Context.ConversationParticipants.Any(p => p.ConversationId == conversationId && p.UserId == userId);
        }

        public PagedList<Message> FindMessages(Guid conversationId, PaginationParams pagination)
        {
            var query = Context.Messages.Where(m => m.ConversationId == conversationId);
            var total = query.Count();
            var items = query
                .Include(m => m.Attachments)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }
    }
}