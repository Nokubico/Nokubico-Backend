using System;
using System.Collections.Generic;
using Nokubico.Domain.Entities;

namespace Nokubico.Domain.Interface.Auth
{
    public interface IAccountRepository
    {
        Account? FindByProviderAndAccountId(string providerId, string accountId);

        List<Account> FindByUser(Guid userId);

        Account Save(Account account);

        void Delete(Account account);
    }
}