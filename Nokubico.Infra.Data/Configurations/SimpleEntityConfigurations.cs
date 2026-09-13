using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    // Small configurations for simple entities to avoid many files when behavior is trivial
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("\"like\"");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.PostId }).IsUnique();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comment");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class ShareConfiguration : IEntityTypeConfiguration<Share>
    {
        public void Configure(EntityTypeBuilder<Share> builder)
        {
            builder.ToTable("share");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.ToTable("bookmark");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("\"order\"");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        }
    }

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_item");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Price).IsRequired();
        }
    }

    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("review");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.ProductId, x.UserId }).IsUnique();
        }
    }

    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("conversations");
            builder.HasKey(x => x.Id);
        }
    }

    public class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.ToTable("conversation_participants");
            builder.HasKey(x => x.Id);
            builder.HasIndex("ConversationId", "UserId").IsUnique();
        }
    }

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("messages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
    {
        public void Configure(EntityTypeBuilder<MessageAttachment> builder)
        {
            builder.ToTable("message_attachments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("company");
            builder.HasKey(x => x.Id);
        }
    }

    public class CompanyMemberConfiguration : IEntityTypeConfiguration<CompanyMember>
    {
        public void Configure(EntityTypeBuilder<CompanyMember> builder)
        {
            builder.ToTable("company_member");
            builder.HasKey(x => x.Id);
            builder.HasIndex("CompanyId", "UserId").IsUnique();
        }
    }

    public class CompanyFollowConfiguration : IEntityTypeConfiguration<CompanyFollow>
    {
        public void Configure(EntityTypeBuilder<CompanyFollow> builder)
        {
            builder.ToTable("company_follow");
            builder.HasKey(x => x.Id);
            builder.HasIndex("CompanyId", "UserId").IsUnique();
        }
    }
}
