using Microsoft.EntityFrameworkCore;

namespace Nokubico.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Domain.Entities.User> Users => Set<Domain.Entities.User>();
        public DbSet<Domain.Entities.Account> Accounts => Set<Domain.Entities.Account>();
        public DbSet<Domain.Entities.Session> Sessions => Set<Domain.Entities.Session>();
        public DbSet<Domain.Entities.Product> Products => Set<Domain.Entities.Product>();
        public DbSet<Domain.Entities.ProductImage> ProductImages => Set<Domain.Entities.ProductImage>();
        public DbSet<Domain.Entities.Post> Posts => Set<Domain.Entities.Post>();
        public DbSet<Domain.Entities.Comment> Comments => Set<Domain.Entities.Comment>();
        public DbSet<Domain.Entities.Like> Likes => Set<Domain.Entities.Like>();
        public DbSet<Domain.Entities.Bookmark> Bookmarks => Set<Domain.Entities.Bookmark>();
        public DbSet<Domain.Entities.Share> Shares => Set<Domain.Entities.Share>();
        public DbSet<Domain.Entities.Order> Orders => Set<Domain.Entities.Order>();
        public DbSet<Domain.Entities.OrderItem> OrderItems => Set<Domain.Entities.OrderItem>();
        public DbSet<Domain.Entities.Review> Reviews => Set<Domain.Entities.Review>();
        public DbSet<Domain.Entities.Conversation> Conversations => Set<Domain.Entities.Conversation>();
        public DbSet<Domain.Entities.ConversationParticipant> ConversationParticipants => Set<Domain.Entities.ConversationParticipant>();
        public DbSet<Domain.Entities.Message> Messages => Set<Domain.Entities.Message>();
        public DbSet<Domain.Entities.MessageAttachment> MessageAttachments => Set<Domain.Entities.MessageAttachment>();
        public DbSet<Domain.Entities.Company> Companies => Set<Domain.Entities.Company>();
        public DbSet<Domain.Entities.CompanyMember> CompanyMembers => Set<Domain.Entities.CompanyMember>();
        public DbSet<Domain.Entities.CompanyFollow> CompanyFollows => Set<Domain.Entities.CompanyFollow>();
        public DbSet<Domain.Entities.Wallet> Wallets => Set<Domain.Entities.Wallet>();
        public DbSet<Domain.Entities.WalletTx> WalletTransactions => Set<Domain.Entities.WalletTx>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
