using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Utils;

namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Handlers;

public static class IdentityHandlers
{
    public static IdentityConfiguration NewIdentityConfiguration(string identityUrl) => new() { IdentityServiceUrl = identityUrl };

    public static IdentityConfiguration WithUsernamePassword(this IdentityConfiguration config, string orchestratorUrl, string username, string password, string tenantName)
    {
        config.OrchestratorUrl = orchestratorUrl;
        config.Username = username;
        config.Password = password;
        config.TenantName = tenantName;

        return config;
    }

    public static IdentityConfiguration WithExternalApp(this IdentityConfiguration config, string clientId, string clientSecret, string scopes)
    {
        config.ClientId = clientId;
        config.ClientSecret = clientSecret;
        config.Scopes = scopes;

        return config;
    }

    public static AccessTokenHandler ExternalAppTokenHandler(this IdentityConfiguration configuration)
        => new ExternalAppTokenHandler(configuration);

    public static AccessTokenHandler UsernamePasswordTokenHandler(this IdentityConfiguration configuration)
        => new UsernamePasswordTokenHandler(configuration);
}
