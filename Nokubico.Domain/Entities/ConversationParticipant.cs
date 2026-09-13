using System;

namespace Nokubico.Domain.Entities
{
    public class ConversationParticipant
    {
        public Guid Id { get; private set; }
        public Guid ConversationId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public DateTime? LastReadAt { get; private set; }

        public void SetConversation(Guid conversationId)
        {
            ConversationId = conversationId;
        }

        public void SetUser(Guid userId)
        {
            UserId = userId;
        }

        public void MarkRead(DateTime at)
        {
            LastReadAt = at;
        }
    }
}
