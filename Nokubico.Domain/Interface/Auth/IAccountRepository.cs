using System;
using System.Collections.Generic;
using Nokubico.Domain.Entities;

namespace Nokubico.Domain.Interface.Auth
{
    public interface IAccountRepository
    {
        Task<Nokubico.Domain.Entities.Account?> FindByProviderAndAccountId(string providerId, string accountId, CancellationToken cancellationToken = default);

        Task<List<Nokubico.Domain.Entities.Account>> FindByUser(Guid userId, CancellationToken cancellationToken = default);

        Task<Nokubico.Domain.Entities.Account> Save(Nokubico.Domain.Entities.Account account, CancellationToken cancellationToken = default);

        Task Delete(Nokubico.Domain.Entities.Account account, CancellationToken cancellationToken = default);
    }
}