using Duende.Bff;
using Marten;
using Marten.Events.Projections;
using Oakton;
using StandardProcess.Api.Auth;
using StandardProcess.Api.User;
using Wolverine;
using Wolverine.Http;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = Path.Combine("wwwroot", "browser")
});

builder.Host.ApplyOaktonExtensions();


builder.Services.AddBff(options =>
{
    // default value is bff
    options.ManagementBasePath = "/api";
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";

}).AddCookie("cookie", options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = false;
    options.Cookie.Name = "__Host-bff";
    options.Cookie.SameSite = SameSiteMode.Strict;

}).AddOpenIdConnect("oidc", options =>
{


    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    
    options.EventsType = typeof(CustomOidcEventTypes);
    options.TokenValidationParameters = new()
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };

});
var connectionString = builder.Configuration.GetConnectionString("database") ?? throw new Exception("We need a database");

builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.Schema.For<UserSession>().Identity(u => u.Key);
    options.Projections.Add<UserSummaryProjection>(ProjectionLifecycle.Inline);


}).UseLightweightSessions().IntegrateWithWolverine();

builder.Host.UseWolverine(opts =>
{
    opts.Policies.UseDurableInboxOnAllListeners();
    opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
    opts.Policies.AutoApplyTransactions();

});
builder.Services.AddTransient<IUserService, CustomUserService>();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseBff();
app.UseDefaultFiles(); // Makes it so index.html is served from the wwwroot/browser directory
app.UseStaticFiles();
app.MapBffManagementEndpoints();
app.UseAuthorization();

app.MapWolverineEndpoints(opts =>
{
    opts.RequireAuthorizeOnAll();
});
if (app.Environment.IsDevelopment())
{
    var uiDevServer = app.Configuration.GetValue<string>("UiDevServerUrl");
    if (!string.IsNullOrEmpty(uiDevServer))
    {
        app.MapReverseProxy();
    }
}

return await app.RunOaktonCommands(args);
