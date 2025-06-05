using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Utils;

namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Handlers;

public class ExternalAppTokenHandler : AccessTokenHandler
{
    private readonly IdentityConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ExternalAppTokenHandler(IdentityConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    protected override async Task RefreshAccessToken(CancellationToken cancellationToken = default)
    {
        var tokenResponse = await _httpClient.GetAccessTokenWithExternalApp(_configuration, cancellationToken);
        SaveAccessTokenFromResponse(tokenResponse);
    }
}
