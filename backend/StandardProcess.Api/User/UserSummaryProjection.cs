using Marten.Events.Aggregation;
namespace StandardProcess.Api.User;

public class UserSummaryProjection : SingleStreamProjection<UserSummary>
{

    public UserSummary Create(UserCreated @event)
    {
        return new UserSummary
        {
            Id = @event.Id,
            LastLogin = DateTimeOffset.Now,
            Sub = @event.Sub,
            Logins = 1,
            Version = 1,
            Authority = @event.Authority
        };

    }
    public void Apply(UserLoggedIn @event, UserSummary model)
    {
        model.Logins++;
        model.Version++;
        model.Authority = @event.Authority;
        model.LastLogin = DateTimeOffset.Now;
    }
}
