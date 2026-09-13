using System;

namespace Nokubico.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
