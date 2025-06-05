namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Utils;

public class IdentityConfiguration
{
    public string IdentityServiceUrl { get; set; }

    public string OrchestratorUrl { get; set; }
    public string TenantName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Scopes { get; set; }

    public string RefreshToken { get; set; }
}
