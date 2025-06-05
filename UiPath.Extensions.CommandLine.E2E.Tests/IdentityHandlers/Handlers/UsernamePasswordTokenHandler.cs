using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Utils;

namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Handlers;

public class UsernamePasswordTokenHandler : AccessTokenHandler
{
    private readonly IdentityConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public UsernamePasswordTokenHandler(IdentityConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration.OrchestratorUrl)
        };
    }

    protected override async Task RefreshAccessToken(CancellationToken cancellationToken = default)
    {
        var accessToken = await _httpClient.GetAccessTokenWithUsernamePassword(_configuration, cancellationToken);
        _accessToken = accessToken ?? string.Empty;
    }
}
