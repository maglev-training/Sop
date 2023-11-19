using Wolverine.Http;
using Wolverine.Http.Marten;

namespace StandardProcess.Api.User;

public static class Api
{
    [WolverineGet("api/users/{id:guid}")]
    public static IResult Get([Document] UserSummary response)
    {
        return Results.Ok(response);
    }
}

