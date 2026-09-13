using System;

namespace Nokubico.Domain.Entities
{
    public class MessageAttachment
    {
        public Guid Id { get; private set; }
        public Guid MessageId { get; private set; }
        public string FileUrl { get; private set; } = null!;
        public string FileName { get; private set; } = null!;
        public string MimeType { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

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
