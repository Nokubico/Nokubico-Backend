using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Wallets;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Wallets
{
    public class WalletRepository : BaseRepository, IWalletRepository
    {
        public WalletRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Wallet?> FindByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);
        }

        public async Task<Wallet?> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Wallets.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<PagedList<WalletTx>> FindTransactions(Guid walletId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.WalletTransactions.Where(t => t.WalletId == walletId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<Wallet> Save(Wallet wallet, CancellationToken cancellationToken = default)
        {
            if (!Context.Wallets.Contains(wallet)) Context.Wallets.Add(wallet);
            await Context.SaveChangesAsync(cancellationToken);
            return wallet;
        }

        public async Task<WalletTx> SaveTransaction(WalletTx transaction, CancellationToken cancellationToken = default)
        {
            if (!Context.WalletTransactions.Contains(transaction)) Context.WalletTransactions.Add(transaction);
            await Context.SaveChangesAsync(cancellationToken);
            return transaction;
        }
    }
}