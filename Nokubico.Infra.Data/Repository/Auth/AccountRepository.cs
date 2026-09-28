using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Auth;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Auth
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Account?> FindByProviderAndAccountId(string providerId, string accountId, CancellationToken cancellationToken = default)
        {
            return await Context.Accounts.FirstOrDefaultAsync(a => a.ProviderId == providerId && a.AccountId == accountId, cancellationToken);
        }

        public async Task<List<Account>> FindByUser(Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Accounts.Where(a => a.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<Account> Save(Account account, CancellationToken cancellationToken = default)
        {
            if (!Context.Accounts.Contains(account)) Context.Accounts.Add(account);
            await Context.SaveChangesAsync(cancellationToken);
            return account;
        }

        public async Task Delete(Account account, CancellationToken cancellationToken = default)
        {
            Context.Accounts.Remove(account);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}