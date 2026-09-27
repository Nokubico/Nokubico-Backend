using System;

namespace Nokubico.Domain.Entities
{
    public class MessageAttachment : IEntity
    {
        public Guid Id { get; private set; }
        public Guid MessageId { get; private set; }
        public string FileUrl { get; private set; } = null!;
        public string FileName { get; private set; } = null!;
        public string MimeType { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        public Message? Message { get; private set; }

        public MessageAttachment()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public MessageAttachment(Message message, string fileUrl, string fileName, string mimeType)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            SetMessage(message);
            SetFile(fileUrl, fileName, mimeType);
        }

        public void SetMessage(Message message)
        {
            Message = message;
            MessageId = message.Id;
        }

        public void SetFile(string url, string name, string mime)
        {
            FileUrl = url;
            FileName = name;
            MimeType = mime;
        }

        public void SetMessage(Guid messageId)
        {
            MessageId = messageId;
        }
    }
}
