using System;

namespace Nokubico.Domain.Entities
{
    public class MessageAttachment
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public string FileUrl { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
