namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Extensions;

public static class HttpClientHandlerExtensions
{
    public static HttpClient CreateHttpClient(this HttpMessageHandler handler, string baseAddress)
    {
        return new HttpClient(handler)
        {
            BaseAddress = new Uri(baseAddress)
        };
    }
}
