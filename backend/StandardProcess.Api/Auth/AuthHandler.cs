using Marten;

namespace StandardProcess.Api.Auth;

public class AuthHandler
{

    public static async Task HandleAsync(ProcessLogin command, ILogger<AuthHandler> logger, IDocumentSession session)
    {
        var user = await session.Query<UserSub>().Where(u => u.Sub == command.Sub).SingleOrDefaultAsync();
        if(user is null)
        {
            var u = new UserSub(Guid.NewGuid(), command.Sub);
            session.Store(u);
            await session.SaveChangesAsync();
        }
        
    }
}

public record UserSub(Guid Id, string Sub);