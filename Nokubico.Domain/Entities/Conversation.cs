using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Conversation : IEntity
    {
        public Guid Id { get; private set; }
        public string? Title { get; private set; }
        public bool IsGroup { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Conversation()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Conversation(string? title, bool isGroup)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Title = title;
            IsGroup = isGroup;
        }

        public ICollection<ConversationParticipant> Participants { get; private set; } = new List<ConversationParticipant>();
        public ICollection<Message> Messages { get; private set; } = new List<Message>();

        public void SetTitle(string? title)
        {
            Title = title;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddParticipant(ConversationParticipant participant)
        {
            if (participant != null) Participants.Add(participant);
        }

        public void AddMessage(Message message)
        {
            if (message != null) Messages.Add(message);
        }
    }
}
