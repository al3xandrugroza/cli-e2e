using System.Net;
using System.Net.Http.Headers;
using IdentityModel.Client;

namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Handlers;

public class AccessTokenHandler : DelegatingHandler
{
    protected string _accessToken;

    public AccessTokenHandler() : base(new HttpClientHandler())
    {
        _accessToken = string.Empty;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await SendRequest();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            response = await SendRequest(refresh: true);
        }

        return response;

        async Task<HttpResponseMessage> SendRequest(bool refresh = false)
        {
            if (refresh)
                await RefreshAccessToken(cancellationToken);

            if (refresh && string.IsNullOrEmpty(_accessToken))
                throw new AccessTokenFetchException("Could not fetch access token. Please check your credentials.");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }

    protected void SaveAccessTokenFromResponse(TokenResponse response) => _accessToken = response.AccessToken ?? string.Empty;

    protected virtual Task RefreshAccessToken(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
