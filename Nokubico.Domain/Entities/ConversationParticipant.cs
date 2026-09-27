using System;

namespace Nokubico.Domain.Entities
{
    public class ConversationParticipant : IEntity
    {
        public Guid Id { get; private set; }
        public Guid ConversationId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public DateTime? LastReadAt { get; private set; }

        public Conversation? Conversation { get; private set; }
        public User? User { get; private set; }

        public ConversationParticipant()
        {
            Id = Guid.NewGuid();
            JoinedAt = DateTime.UtcNow;
        }

        public ConversationParticipant(Conversation conversation, User user)
        {
            Id = Guid.NewGuid();
            JoinedAt = DateTime.UtcNow;
            SetConversation(conversation);
            SetUser(user);
        }

        public void SetConversation(Conversation conversation)
        {
            Conversation = conversation;
            ConversationId = conversation.Id;
        }

        public void SetUser(User user)
        {
            User = user;
            UserId = user.Id;
        }

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
