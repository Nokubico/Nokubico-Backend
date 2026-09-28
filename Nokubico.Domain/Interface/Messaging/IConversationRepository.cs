using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Messaging
{
    public interface IConversationRepository
    {
        Task<Conversation?> FindById(Guid id, CancellationToken cancellationToken = default);

        Task<PagedList<Conversation>> FindByUser(Guid userId, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<Conversation> Save(Conversation conversation, CancellationToken cancellationToken = default);

        Task<Message> SaveMessage(Message message, CancellationToken cancellationToken = default);

        Task<MessageAttachment> SaveAttachment(MessageAttachment attachment, CancellationToken cancellationToken = default);

        Task<bool> IsParticipant(Guid conversationId, Guid userId, CancellationToken cancellationToken = default);

        Task<PagedList<Message>> FindMessages(Guid conversationId, PaginationParams pagination, CancellationToken cancellationToken = default);
    }
}