using System;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; private set; }
        public Guid ConversationId { get; private set; }
        public Guid? SenderId { get; private set; }
        public string? Content { get; private set; }
        public string? MessageType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Guid? ReplyToId { get; private set; }

        public void SetContent(string? content)
        {
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetSender(Guid? senderId)
        {
            SenderId = senderId;
        }

        public void SetConversation(Guid conversationId)
        {
            ConversationId = conversationId;
        }
    }
}
