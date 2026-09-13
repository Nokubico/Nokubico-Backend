using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ConversationParticipant>? Participants { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
