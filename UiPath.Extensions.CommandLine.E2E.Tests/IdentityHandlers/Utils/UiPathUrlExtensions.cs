namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Utils;

public static class UiPathUrlExtensions
{
    private const string DefaultIdentity = "identity_/";
    private const string MsiIdentity = "identity/";

    public static string AsIdentityServiceUrl(this string baseUrl, bool isMsiDeployment = false)
        => ConcatAsUri(baseUrl, isMsiDeployment ? MsiIdentity : DefaultIdentity);
    
    
    public static string ConcatAsUri(this string baseUrl, string relative)
    {
        var baseUrlForUriConcat = baseUrl.EndsWith('/') ? baseUrl : baseUrl + "/";
        var relativeForUriConcat = relative.StartsWith('/') ? relative[1..] : relative;
        
        return new Uri(new Uri(baseUrlForUriConcat), relativeForUriConcat).AbsoluteUri;
    } 
    
    internal static string AsConnectToken(this string identityServiceUrl) => ConcatAsUri(identityServiceUrl, "connect/token");

    internal static string AsOauthToken(this string identityServiceUrl) => ConcatAsUri(identityServiceUrl, "oauth/token");
}
