using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nokubico.Domain.Entities;

namespace Nokubico.Infra.Data.Configurations
{
    // Agrupa as configurações das entidades simples.
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("\"like\"");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Post).WithMany(p => p.Likes).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
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
            builder.HasOne(x => x.Author).WithMany().HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Post).WithMany(p => p.Comments).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class ShareConfiguration : IEntityTypeConfiguration<Share>
    {
        public void Configure(EntityTypeBuilder<Share> builder)
        {
            builder.ToTable("share");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Post).WithMany().HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => new { x.UserId, x.PostId }).IsUnique();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.ToTable("bookmark");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Post).WithMany().HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => new { x.UserId, x.PostId }).IsUnique();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("\"order\"");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
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
            builder.HasOne(x => x.Order).WithMany(o => o.Items).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Product).WithMany(p => p.OrderItems).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.Price).IsRequired();
        }
    }

    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("review");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Product).WithMany(p => p.Reviews).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
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
            builder.HasOne(x => x.Conversation).WithMany(c => c.Participants).HasForeignKey(x => x.ConversationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex("ConversationId", "UserId").IsUnique();
        }
    }

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("messages");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Conversation).WithMany(c => c.Messages).HasForeignKey(x => x.ConversationId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        }
    }

    public class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
    {
        public void Configure(EntityTypeBuilder<MessageAttachment> builder)
        {
            builder.ToTable("message_attachments");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Message).WithMany(m => m.Attachments).HasForeignKey(x => x.MessageId).OnDelete(DeleteBehavior.Cascade);
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
            builder.HasOne(x => x.Company).WithMany(c => c.Members).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex("CompanyId", "UserId").IsUnique();
        }
    }

    public class CompanyFollowConfiguration : IEntityTypeConfiguration<CompanyFollow>
    {
        public void Configure(EntityTypeBuilder<CompanyFollow> builder)
        {
            builder.ToTable("company_follow");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Company).WithMany(c => c.Follows).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex("CompanyId", "UserId").IsUnique();
        }
    }
}
