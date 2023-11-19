using Marten;
using StandardProcess.Api.User;

namespace StandardProcess.Api.Auth;

public class AuthHandler
{

    public static async Task HandleAsync(ProcessLogin command, ILogger<AuthHandler> logger, IDocumentSession session)
    {
        var user = await session.Query<UserSummary>().Where(u => u.Sub == command.Sub).SingleOrDefaultAsync();
        if(user is null)
        {
            var u = new UserSub(Guid.NewGuid(), command.Sub);
            session.Store(u);
            session.Events.Append(u.Id, new UserCreated(u.Id, command.Sub, ""));
        } else
        {
            session.Events.Append(user.Id, new UserLoggedIn(user.Id));
        }
        
            await session.SaveChangesAsync();
    }
}

public record UserSub(Guid Id, string Sub);