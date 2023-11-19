using Marten;
using Marten.Events.Aggregation;
using Marten.Events.Projections;
using Wolverine.Http;
namespace StandardProcess.Api.User;

public static  class Api
{
    [WolverineGet("api/users/{id:guid}")]
 
   public static async Task<IResult> GetAsync(Guid id, IQuerySession session, CancellationToken token)
    {

        var response = await session.LoadAsync<UserSummary>(id, token);
        return Results.Ok(response);
    }
}

public class UserSummary {
    public Guid Id { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    public string Sub { get; set; } = string.Empty;
    public int Logins { get; set; }
    public int Version { get; set; }

}

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
            Version = 1
        };

    }
    public void Apply(UserLoggedIn @event, UserSummary model)
    {
        model.Logins++;
        model.Version++;
        model.LastLogin = DateTimeOffset.Now;
    }
}

public record UserCreated(Guid Id, string Sub, string Authority);
public record UserLoggedIn(Guid Id);