using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Wallets
{
    public interface IWalletRepository
    {
        Task<Wallet?> FindByUserId(Guid userId, CancellationToken cancellationToken = default);

        Task<Wallet?> FindById(Guid id, CancellationToken cancellationToken = default);

        Task<PagedList<WalletTx>> FindTransactions(Guid walletId, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<Wallet> Save(Wallet wallet, CancellationToken cancellationToken = default);

        Task<WalletTx> SaveTransaction(WalletTx transaction, CancellationToken cancellationToken = default);
    }
}