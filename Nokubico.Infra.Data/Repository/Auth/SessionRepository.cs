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

        public async Task<Session?> FindActiveByToken(string token, CancellationToken cancellationToken = default)
        {
            return await Context.Sessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow, cancellationToken);
        }

        public async Task<Session> Save(Session session, CancellationToken cancellationToken = default)
        {
            if (!Context.Sessions.Contains(session)) Context.Sessions.Add(session);
            await Context.SaveChangesAsync(cancellationToken);
            return session;
        }

        public async Task Delete(Session session, CancellationToken cancellationToken = default)
        {
            Context.Sessions.Remove(session);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAllForUser(Guid userId, CancellationToken cancellationToken = default)
        {
            await Context.Sessions.Where(s => s.UserId == userId).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<long> DeleteExpired(DateTime now, CancellationToken cancellationToken = default)
        {
            return await Context.Sessions.Where(s => s.ExpiresAt < now).ExecuteDeleteAsync(cancellationToken);
        }
    }
}