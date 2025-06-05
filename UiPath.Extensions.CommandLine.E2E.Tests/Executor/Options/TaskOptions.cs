using UiPath.Extensions.CommandLine.E2E.Tests.TelemetryEnums;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;

public abstract class TaskOptions
{
    public string AuthorizationUrl { get; set; }

    public string OrchestratorUrl { get; set; }
    public string AccountName { get; set; }
    public string AccountForApp { get; set; }
    public string OrchestratorTenant { get; set; }
    public string OrganizationUnit { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }

    public string RefreshToken { get; set; }

    public string ApplicationId { get; set; }
    public string ApplicationSecret { get; set; }
    public string ApplicationScope { get; set; }


    public string Language { get; set; }
    public TracingLevel? TraceLevel { get; set; }
    public bool DisableTelemetry { get; set; }


    public TelemetryOrigin TelemetryOrigin { get; set; }
    public string TelemetryOriginVersion { get; set; }
    public string PipelineCorrelationId { get; set; }
    public string ExtensionClientOrganizationId { get; set; }
    public string ExtensionClientProjectId { get; set; }


    public string DummyOption { get; } = "This dummy additional option should not break the cli. The cli should ignore it.";

    public abstract string GetInlineCommandArgs();

    public abstract string GetInlineShortCommandArgs();
}
