using System;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? SenderId { get; set; }
        public string? Content { get; set; }
        public string? MessageType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? ReplyToId { get; set; }
    }
}
