using Duende.Bff;
using Marten;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using StandardProcess.Api.User;
using System.Collections.ObjectModel;

namespace StandardProcess.Api.Auth;

public class CustomUserService : DefaultUserService
{
    private readonly IDocumentSession _session;
    private  Guid _userId;
    public CustomUserService(IOptions<BffOptions> options, ILoggerFactory loggerFactory, IDocumentSession session) : base(options, loggerFactory)
    {
        _session = session;
    }
    public override async Task ProcessRequestAsync(HttpContext context)
    {
        var identity = context.User.Identity;
        if ( identity is not null && identity.IsAuthenticated)
        {
            var sub = context.User.Claims.SingleOrDefault(c => c.Type == "sub");

            if (sub is not null)
            {
                var user = await _session.Query<UserSummary>().SingleOrDefaultAsync(u => u.Sub == sub.Value);
                if(user is not null)
                {
                    _userId = user.Id;
                }
            }
        }
       
        await base.ProcessRequestAsync(context);
    }
    protected override IEnumerable<ClaimRecord> GetManagementClaims(HttpContext context, AuthenticateResult authenticateResult)
    {
        return base.GetManagementClaims(context, authenticateResult);
    }

    protected override IEnumerable<ClaimRecord> GetUserClaims(AuthenticateResult authenticateResult)
    {
        var claims = base.GetUserClaims(authenticateResult);
        var streamId = new ClaimRecord("stream_id", _userId);
        return [streamId, .. claims];
    }
}

