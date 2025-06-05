using System.Diagnostics;
using System.Text;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;

internal class AnalyzeOptions : TaskOptions
{
    public string ProjectPath { get; set; }
    public string IgnoredRules { get; set; }
    public bool StopOnRuleViolation { get; set; }
    public bool DisableBuiltInNugetFeeds { get; set; }
    public bool TreatWarningsAsErrors { get; set; }
    public TraceLevel? AnalyzerTraceLevel { get; set; }
    public string ResultFilePath { get; set; }
    public string GovernanceFilePath { get; set; }

    public override string GetInlineCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"package analyze \"{ProjectPath}\"");
        if (AnalyzerTraceLevel is not null)
            commandArgs.Append($" --analyzerTraceLevel \"{AnalyzerTraceLevel}\"");
        if (StopOnRuleViolation)
            commandArgs.Append($" --stopOnRuleViolation");
        if (DisableBuiltInNugetFeeds)
            commandArgs.Append($" --disableBuiltInNugetFeeds");
        if (TreatWarningsAsErrors)
            commandArgs.Append($" --treatWarningsAsErrors");
        if (ResultFilePath is not null)
            commandArgs.Append($" --resultPath \"{ResultFilePath}\"");
        if (GovernanceFilePath is not null)
            commandArgs.Append($" --governanceFilePath \"{GovernanceFilePath}\"");
        if (IgnoredRules is not null)
            commandArgs.Append($" --ignoredRules \"{IgnoredRules}\"");
        if (Username is not null)
            commandArgs.Append($" -u \"{Username}\"");
        if (Password is not null)
            commandArgs.Append($" -p \"{Password}\"");
        if (RefreshToken is not null)
            commandArgs.Append($" -t \"{RefreshToken}\"");
        if (AccountName is not null)
            commandArgs.Append($" -a \"{AccountName}\"");
        if (AccountForApp is not null)
            commandArgs.Append($" -A \"{AccountForApp}\"");
        if (ApplicationId is not null)
            commandArgs.Append($" -I \"{ApplicationId}\"");
        if (ApplicationSecret is not null)
            commandArgs.Append($" -S \"{ApplicationSecret}\"");
        if (ApplicationScope is not null)
            commandArgs.Append($" --orchestratorApplicationScope \"{ApplicationScope}\"");
        if (OrganizationUnit is not null)
            commandArgs.Append($" --orchestratorFolder \"{OrganizationUnit}\"");
        if (OrchestratorUrl is not null)
            commandArgs.Append($" --orchestratorUrl \"{OrchestratorUrl}\"");
        if (OrchestratorTenant is not null)
            commandArgs.Append($" --orchestratorTenant \"{OrchestratorTenant}\"");
        if (Language is not null)
            commandArgs.Append($" --language \"{Language}\"");
        if (DisableTelemetry)
            commandArgs.Append($" --disableTelemetry");
        if (TraceLevel is not null)
            commandArgs.Append($" --traceLevel \"{TraceLevel}\"");
        if (AuthorizationUrl is not null)
            commandArgs.Append($"  --identityUrl \"{AuthorizationUrl}\"");

        return commandArgs.ToString();
    }

    public override string GetInlineShortCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"package analyze \"{ProjectPath}\"");
        if (AnalyzerTraceLevel is not null)
            commandArgs.Append($" --analyzerTraceLevel \"{AnalyzerTraceLevel}\"");
        if (StopOnRuleViolation)
            commandArgs.Append($" --stopOnRuleViolation");
        if (DisableBuiltInNugetFeeds)
            commandArgs.Append($" --disableBuiltInNugetFeeds");
        if (TreatWarningsAsErrors)
            commandArgs.Append($" --treatWarningsAsErrors");
        if (ResultFilePath is not null)
            commandArgs.Append($" --resultPath \"{ResultFilePath}\"");
        if (GovernanceFilePath is not null)
            commandArgs.Append($" --governanceFilePath \"{GovernanceFilePath}\"");
        if (IgnoredRules is not null)
            commandArgs.Append($" --ignoredRules \"{IgnoredRules}\"");
        if (Username is not null)
            commandArgs.Append($" --orchestratorUsername \"{Username}\"");
        if (Password is not null)
            commandArgs.Append($" --orchestratorPassword \"{Password}\"");
        if (RefreshToken is not null)
            commandArgs.Append($" --orchestratorAuthToken \"{RefreshToken}\"");
        if (AccountName is not null)
            commandArgs.Append($" --orchestratorAccountName \"{AccountName}\"");
        if (AccountForApp is not null)
            commandArgs.Append($" --orchestratorAccountForApp \"{AccountForApp}\"");
        if (ApplicationId is not null)
            commandArgs.Append($" --orchestratorApplicationId \"{ApplicationId}\"");
        if (ApplicationSecret is not null)
            commandArgs.Append($" --orchestratorApplicationSecret \"{ApplicationSecret}\"");
        if (ApplicationScope is not null)
            commandArgs.Append($" --orchestratorApplicationScope \"{ApplicationScope}\"");
        if (OrganizationUnit is not null)
            commandArgs.Append($" --orchestratorFolder \"{OrganizationUnit}\"");
        if (OrchestratorUrl is not null)
            commandArgs.Append($" --orchestratorUrl \"{OrchestratorUrl}\"");
        if (OrchestratorTenant is not null)
            commandArgs.Append($" --orchestratorTenant \"{OrchestratorTenant}\"");
        if (Language is not null)
            commandArgs.Append($" -l \"{Language}\"");
        if (DisableTelemetry)
            commandArgs.Append($" -y");
        if (TraceLevel is not null)
            commandArgs.Append($" --traceLevel \"{TraceLevel}\"");
        if (AuthorizationUrl is not null)
            commandArgs.Append($"  --identityUrl \"{AuthorizationUrl}\"");

        return commandArgs.ToString();
    }
}
