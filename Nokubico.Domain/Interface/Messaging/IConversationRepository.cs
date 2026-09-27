using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Messaging
{
    public interface IConversationRepository
    {
        Conversation? FindById(Guid id);

        PagedList<Conversation> FindByUser(Guid userId, PaginationParams pagination);

        Conversation Save(Conversation conversation);

        Message SaveMessage(Message message);

        MessageAttachment SaveAttachment(MessageAttachment attachment);

        bool IsParticipant(Guid conversationId, Guid userId);

        PagedList<Message> FindMessages(Guid conversationId, PaginationParams pagination);
    }
}