using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Walletss;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Wallets
{
    public class WalletRepository : BaseRepository, IWalletRepository
    {
        public WalletRepository(AppDbContext context) : base(context)
        {
        }

        public Wallet? FindByUserId(Guid userId)
        {
            return Context.Wallets.FirstOrDefault(w => w.UserId == userId);
        }

        public Wallet? FindById(Guid id)
        {
            return Context.Wallets.FirstOrDefault(w => w.Id == id);
        }

        public PagedList<WalletTx> FindTransactions(Guid walletId, PaginationParams pagination)
        {
            var query = Context.WalletTransactions.Where(t => t.WalletId == walletId);
            var total = query.Count();
            var items = query
                .OrderByDescending(t => t.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public Wallet Save(Wallet wallet)
        {
            if (!Context.Wallets.Contains(wallet)) Context.Wallets.Add(wallet);
            Context.SaveChanges();
            return wallet;
        }

        public WalletTx SaveTransaction(WalletTx transaction)
        {
            if (!Context.WalletTransactions.Contains(transaction)) Context.WalletTransactions.Add(transaction);
            Context.SaveChanges();
            return transaction;
        }
    }
}