using System;
using System.Collections.Generic;
using Nokubico.Domain.Entities;

namespace Nokubico.Domain.Interface.Auth
{
    public interface IAccountRepository
    {
        Task<Entities.Account?> FindByProviderAndAccountId(string providerId, string accountId, CancellationToken cancellationToken = default);

        Task<List<Entities.Account>> FindByUser(Guid userId, CancellationToken cancellationToken = default);

        Task<Entities.Account> Save(Entities.Account account, CancellationToken cancellationToken = default);

        Task Delete(Entities.Account account, CancellationToken cancellationToken = default);
    }
}