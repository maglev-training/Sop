using Duende.Bff;
using Marten;

/*
 * I am not yet using server sessions. If I do, I *think* this is an adequate replacement for the EF version from the box.
 */
namespace StandardProcess.Api.Auth;

public class CustomUserSessionStore : IUserSessionStore
{
    private readonly IDocumentSession _session;

    public CustomUserSessionStore(IDocumentSession session)
    {
        _session = session;
    }

    public Task CreateUserSessionAsync(UserSession session, CancellationToken cancellationToken = default)
    {
        _session.Store(session);
        return _session.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteUserSessionAsync(string key, CancellationToken cancellationToken = default)
    {
        _session.Delete(key);
        return _session.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteUserSessionsAsync(UserSessionsFilter filter, CancellationToken cancellationToken = default)
    {
        _session.DeleteWhere<UserSession>(u => u.SubjectId == filter.SubjectId || u.SessionId == filter.SessionId);
        return _session.SaveChangesAsync(cancellationToken);
    } 

    public Task<UserSession?> GetUserSessionAsync(string key, CancellationToken cancellationToken = default)
    {
        return _session.LoadAsync<UserSession>(key, cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserSession>> GetUserSessionsAsync(UserSessionsFilter filter, CancellationToken cancellationToken = default)
    {
        var results=   _session.Query<UserSession>().Where(u => u.SessionId == filter.SessionId || u.SubjectId == filter.SubjectId).ToListAsync(cancellationToken);
        return await results;
    }

    public async Task UpdateUserSessionAsync(string key, UserSessionUpdate session, CancellationToken cancellationToken = default)
    {
        var stored = await _session.LoadAsync<UserSession>(session.SubjectId, cancellationToken);
        if(stored is not null)
        {
            session.CopyTo(stored);
            _session.Store(session);
            await _session.SaveChangesAsync(cancellationToken);
        }
    }
}