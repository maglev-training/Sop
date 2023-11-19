﻿using Duende.Bff;
using Wolverine;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Marten;

namespace StandardProcess.Api.Auth;

public class CustomOidcEventTypes : BffOpenIdConnectEvents
{
    private readonly IMessageBus _bus;
    private readonly IDocumentSession _session;
    public CustomOidcEventTypes(ILogger<BffOpenIdConnectEvents> logger, IMessageBus bus, IDocumentSession session) : base(logger)
    {
        _bus = bus;
        _session = session;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var identity = context?.Principal?.Identity;
        if (context?.Principal is not null && identity is not null && identity.IsAuthenticated)
        {
            var sub = context.Principal.Claims.SingleOrDefault(c => c.Type == "sub");
           
            if (sub is not null)
            {
                await _bus.PublishAsync(new ProcessLogin(sub.Value));
                
            }
        }

        await base.TokenValidated(context!);
    }

}