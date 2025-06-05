using UiPath.Orchestrator.Web.ClientV3;
using IdentityModel.Client;
using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Utils;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;

namespace UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Extensions;

public static class HttpClientExtensions
{
    public static Task<TokenResponse> GetAccessTokenWithExternalApp(this HttpClient httpClient, IdentityConfiguration configuration, CancellationToken cancellationToken = default)
        => httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        {
            ClientId = configuration.ClientId,
            ClientSecret = configuration.ClientSecret,
            GrantType = "client_credentials",
            Scope = configuration.Scopes,
            Address = configuration.IdentityServiceUrl.AsConnectToken(),
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
        }, cancellationToken);


    public static Task<TokenResponse> GetAccessTokenWithRefreshToken(this HttpClient httpClient, IdentityConfiguration configuration, CancellationToken cancellationToken = default)
        => httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
        {
            ClientId = configuration.ClientId,
            RefreshToken = configuration.RefreshToken,
            GrantType = "refresh_token",
            Address = configuration.IdentityServiceUrl.AsOauthToken(),
            ClientCredentialStyle = ClientCredentialStyle.PostBody
        }, cancellationToken: cancellationToken);

    public static async Task<string> GetAccessTokenWithUsernamePassword(this HttpClient httpClient, IdentityConfiguration configuration, CancellationToken cancellationToken = default)
    {
        var accountClient = new AccountClient(new NSwagClientConfig(), httpClient);
        var loginModel = new LoginModel
        {
            TenancyName = configuration.TenantName,
            UsernameOrEmailAddress = configuration.Username,
            Password = configuration.Password
        };
        var response = await accountClient.AuthenticateAsync(loginModel, cancellationToken: cancellationToken);
        return response?.Body.Result.ToString();
    }

    public static HttpClient AddOrganizationUnitHeader(this HttpClient httpClient, string organizationUnitId)
    {
        return httpClient.AddHeader(OrchestratorClientConstants.OrganizationUnitIdHeader, organizationUnitId);
    }
    
    public static HttpClient AddFolderPathHeader(this HttpClient httpClient, string folderPath)
    {
        return httpClient.AddHeader(OrchestratorClientConstants.FolderPath, folderPath);
    }
    
    public static HttpClient AddHeader(this HttpClient httpClient, string headerName, string headerValue)
    {
        if (httpClient.DefaultRequestHeaders.Contains(headerName))
            httpClient.DefaultRequestHeaders.Remove(headerName);
        httpClient.DefaultRequestHeaders.Add(headerName, headerValue);

        return httpClient;
    }
}
