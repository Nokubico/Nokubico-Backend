using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Auth;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Auth
{
    public class SessionRepository : BaseRepository, ISessionRepository
    {
        public SessionRepository(AppDbContext context) : base(context)
        {
        }

        public Session? FindActiveByToken(string token)
        {
            return Context.Sessions
                .Include(s => s.User)
                .FirstOrDefault(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);
        }

        public Session Save(Session session)
        {
            if (!Context.Sessions.Contains(session)) Context.Sessions.Add(session);
            Context.SaveChanges();
            return session;
        }

        public void Delete(Session session)
        {
            Context.Sessions.Remove(session);
            Context.SaveChanges();
        }

        public void DeleteAllForUser(Guid userId)
        {
            Context.Sessions.Where(s => s.UserId == userId).ExecuteDelete();
        }

        public long DeleteExpired(DateTime now)
        {
            return Context.Sessions.Where(s => s.ExpiresAt < now).ExecuteDelete();
        }
    }
}