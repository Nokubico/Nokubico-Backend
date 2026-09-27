using System;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class Message : IEntity
    {
        public Guid Id { get; private set; }
        public Guid ConversationId { get; private set; }
        public Guid? SenderId { get; private set; }
        public string? Content { get; private set; }
        public string? MessageType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Guid? ReplyToId { get; private set; }

        public Conversation? Conversation { get; private set; }
        public ICollection<MessageAttachment> Attachments { get; private set; } = new List<MessageAttachment>();

        public Message()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Message(Guid conversationId, Guid? senderId, string? content, string? messageType)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ConversationId = conversationId;
            SenderId = senderId;
            Content = content;
            MessageType = messageType;
        }

        public void SetConversation(Conversation conversation)
        {
            Conversation = conversation;
            ConversationId = conversation.Id;
        }

        public void AddAttachment(MessageAttachment attachment)
        {
            if (attachment != null) Attachments.Add(attachment);
        }

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
