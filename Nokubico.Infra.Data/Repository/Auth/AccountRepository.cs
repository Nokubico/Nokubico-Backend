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

        public Account? FindByProviderAndAccountId(string providerId, string accountId)
        {
            return Context.Accounts.FirstOrDefault(a => a.ProviderId == providerId && a.AccountId == accountId);
        }

        public List<Account> FindByUser(Guid userId)
        {
            return Context.Accounts.Where(a => a.UserId == userId).ToList();
        }

        public Account Save(Account account)
        {
            if (!Context.Accounts.Contains(account)) Context.Accounts.Add(account);
            Context.SaveChanges();
            return account;
        }

        public void Delete(Account account)
        {
            Context.Accounts.Remove(account);
            Context.SaveChanges();
        }
    }
}