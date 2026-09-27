using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Walletss
{
    public interface IWalletRepository
    {
        Wallet? FindByUserId(Guid userId);

        Wallet? FindById(Guid id);

        PagedList<WalletTx> FindTransactions(Guid walletId, PaginationParams pagination);

        Wallet Save(Wallet wallet);

        WalletTx SaveTransaction(WalletTx transaction);
    }
}