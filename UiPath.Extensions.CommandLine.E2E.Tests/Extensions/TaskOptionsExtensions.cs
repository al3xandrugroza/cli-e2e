using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TelemetryEnums;
using UiPath.Extensions.CommandLine.E2E.Tests.Utils;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Extensions;

internal static class TaskOptionsExtensions
{
    public static TaskOptions PopulateWithConnectionDetails(this TaskOptions options, OrchestratorConnection? connection)
    {
        if (connection == null)
            return options;

        options.AuthorizationUrl = connection.AuthorizationUrl;

        options.OrchestratorUrl = connection.BaseUrl;
        options.AccountName = connection.AccountName;
        options.AccountForApp = connection.AccountForApp;
        options.OrchestratorTenant = connection.OrchestratorTenant;
        options.OrganizationUnit = connection.OrganizationUnit;

        options.Username = connection.Username;
        options.Password = connection.Password;

        options.RefreshToken = connection.RefreshToken;
                
        options.ApplicationId = connection.ApplicationId;
        options.ApplicationSecret = connection.ApplicationSecret;
        options.ApplicationScope = connection.ApplicationScope;

        return options;
    }

    public static TaskOptions PopulateWithConfigurationDetails(this TaskOptions options, string language = "en-US", TracingLevel? traceLevel = TracingLevel.Information, bool disableTelemetry = false)
    {
        options.Language = language;
        options.TraceLevel = traceLevel;
        options.DisableTelemetry = disableTelemetry;
        
        return options;
    }

    public static TaskOptions PopulateWithTelemetryData(this TaskOptions options)
    {
        options.TelemetryOrigin = TelemetryOrigin.E2E;
        options.TelemetryOriginVersion = AssemblyDetails.GetProductVersion();
        options.PipelineCorrelationId = Guid.NewGuid().ToString();
        options.ExtensionClientOrganizationId = Guid.NewGuid().ToString();
        options.ExtensionClientProjectId = Guid.NewGuid().ToString();

        return options;
    }
}
