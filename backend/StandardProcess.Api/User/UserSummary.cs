namespace StandardProcess.Api.User;

public class UserSummary {
    public Guid Id { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    public string Sub { get; set; } = string.Empty;
    public int Logins { get; set; }
    public int Version { get; set; }
    public string Authority { get; set; } = string.Empty;

}
