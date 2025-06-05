using System.Text;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options.Enums;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;

internal class PackOptions : TaskOptions
{
    public string ProjectPath { get; set; }
    public OutputType? OutputType { get; set; }
    public bool SplitOutput { get; set; }
    public bool AutoVersion { get; set; }
    public bool DisableBuiltInNugetFeeds { get; set; }
    public string Version { get; set; }
    public string DestinationFolder { get; set; }
    public string? RepositoryUrl { get; set; }
    public string? RepositoryCommit { get; set; }
    public string? RepositoryBranch { get; set; }
    public string? RepositoryType { get; set; }
    public string? ProjectUrl { get; set; }
    public string? ReleaseNotes { get; set; }

    public override string GetInlineCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"package pack \"{ProjectPath}\"");
        if (DestinationFolder is not null)
            commandArgs.Append($" --output \"{DestinationFolder}\"");
        if (Version is not null)
            commandArgs.Append($" --version \"{Version}\"");
        if (AutoVersion)
            commandArgs.Append($" --autoVersion");
        if (DisableBuiltInNugetFeeds)
            commandArgs.Append($" --disableBuiltInNugetFeeds");
        if (OutputType is not null)
            commandArgs.Append($" --outputType \"{OutputType}\"");
        if (SplitOutput)
            commandArgs.Append($" --splitOutput");
        if (RepositoryUrl is not null) 
            commandArgs.Append($" --repositoryUrl \"{RepositoryUrl}\"");
        if (RepositoryCommit is not null)
            commandArgs.Append($" --repositoryCommit \"{RepositoryCommit}\"");
        if (RepositoryBranch is not null)
            commandArgs.Append($" --repositoryBranch \"{RepositoryBranch}\"");
        if (RepositoryType is not null)
            commandArgs.Append($" --repositoryType \"{RepositoryType}\"");
        if (ProjectUrl is not null)
            commandArgs.Append($" --projectUrl \"{ProjectUrl}\"");
        if (ReleaseNotes is not null)
            commandArgs.Append($" --releaseNotes \"{ReleaseNotes}\"");
        if (Username is not null)
            commandArgs.Append($" --libraryOrchestratorUsername \"{Username}\"");
        if (Password is not null)
            commandArgs.Append($" --libraryOrchestratorPassword \"{Password}\"");
        if (RefreshToken is not null)
            commandArgs.Append($" --libraryOrchestratorAuthToken \"{RefreshToken}\"");
        if (AccountName is not null)
            commandArgs.Append($" --libraryOrchestratorAccountName \"{AccountName}\"");
        if (AccountForApp is not null)
            commandArgs.Append($" --libraryOrchestratorAccountForApp \"{AccountForApp}\"");
        if (ApplicationId is not null)
            commandArgs.Append($" --libraryOrchestratorApplicationId \"{ApplicationId}\"");
        if (ApplicationSecret is not null)
            commandArgs.Append($" --libraryOrchestratorApplicationSecret \"{ApplicationSecret}\"");
        if (ApplicationScope is not null)
            commandArgs.Append($" --libraryOrchestratorApplicationScope \"{ApplicationScope}\"");
        if (OrganizationUnit is not null)
            commandArgs.Append($" --libraryOrchestratorFolder \"{OrganizationUnit}\"");
        if (OrchestratorUrl is not null)
            commandArgs.Append($" --libraryOrchestratorUrl \"{OrchestratorUrl}\"");
        if (OrchestratorTenant is not null)
            commandArgs.Append($" --libraryOrchestratorTenant \"{OrchestratorTenant}\"");
        if (DisableTelemetry)
            commandArgs.Append($" --disableTelemetry");
        if (TraceLevel is not null)
            commandArgs.Append($" --traceLevel \"{TraceLevel}\"");
        if (Language is not null)
            commandArgs.Append($" --language \"{Language}\"");
        if (AuthorizationUrl is not null)
            commandArgs.Append($" --libraryIdentityUrl \"{AuthorizationUrl}\"");

        return commandArgs.ToString();
    }

    public override string GetInlineShortCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"package pack \"{ProjectPath}\"");
        if (DestinationFolder is not null)
            commandArgs.Append($" -o \"{DestinationFolder}\"");
        if (Version is not null)
            commandArgs.Append($" -v \"{Version}\"");
        if (AutoVersion)
            commandArgs.Append($" --autoVersion");
        if (OutputType is not null)
            commandArgs.Append($" --outputType \"{OutputType}\"");
        if (SplitOutput)
            commandArgs.Append($" --splitOutput");
        if (RepositoryUrl is not null) 
            commandArgs.Append($" --repositoryUrl \"{RepositoryUrl}\"");
        if (RepositoryCommit is not null)
            commandArgs.Append($" --repositoryCommit \"{RepositoryCommit}\"");
        if (RepositoryBranch is not null)
            commandArgs.Append($" --repositoryBranch \"{RepositoryBranch}\"");
        if (RepositoryType is not null)
            commandArgs.Append($" --repositoryType \"{RepositoryType}\"");
        if (ProjectUrl is not null)
            commandArgs.Append($" --projectUrl \"{ProjectUrl}\"");
        if (ReleaseNotes is not null)
            commandArgs.Append($" --releaseNotes \"{ReleaseNotes}\"");
        if (DisableBuiltInNugetFeeds)
            commandArgs.Append($" --disableBuiltInNugetFeeds");
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
            commandArgs.Append($" --libraryOrchestratorApplicationScope \"{ApplicationScope}\"");
        if (OrganizationUnit is not null)
            commandArgs.Append($" --libraryOrchestratorFolder \"{OrganizationUnit}\"");
        if (OrchestratorUrl is not null)
            commandArgs.Append($" --libraryOrchestratorUrl \"{OrchestratorUrl}\"");
        if (OrchestratorTenant is not null)
            commandArgs.Append($" --libraryOrchestratorTenant \"{OrchestratorTenant}\"");
        if (DisableTelemetry)
            commandArgs.Append($" -y");
        if (TraceLevel is not null)
            commandArgs.Append($" --traceLevel \"{TraceLevel}\"");
        if (Language is not null)
            commandArgs.Append($" -l \"{Language}\"");
        if (AuthorizationUrl is not null)
            commandArgs.Append($" --libraryIdentityUrl \"{AuthorizationUrl}\"");

        return commandArgs.ToString();
    }
}
