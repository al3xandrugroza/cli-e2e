using System.Net;
using System.Text;

namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Handlers;

public class ExtractAccessTokenHandler : DelegatingHandler
{
    private readonly StringBuilder _accessToken;
    public ExtractAccessTokenHandler(HttpMessageHandler innerHandler, StringBuilder accessToken) : base(innerHandler)
    {
        _accessToken = accessToken;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized && request.Headers.TryGetValues("Authorization", out var headerValues))
        {
            const string BearerPrefix = "Bearer ";
            var accessToken = headerValues.FirstOrDefault();
            if (accessToken == null || accessToken.Length <= BearerPrefix.Length)
                return response;

            _accessToken.Clear();
            _accessToken.Append(accessToken.AsSpan(BearerPrefix.Length));
        }

        return response;
    }
}
