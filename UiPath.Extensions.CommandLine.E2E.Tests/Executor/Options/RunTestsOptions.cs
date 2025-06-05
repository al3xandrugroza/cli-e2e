using System.Text;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options.Enums;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;

internal class RunTestsOptions : TaskOptions
{
    public string ProjectPath { get; set; }
    public string TestSet { get; set; }

    public string Environment { get; set; }
    public TestReportType? TestReportType { get; set; }
    public string TestReportDestination { get; set; }

    public int? RetryCount { get; set; }
    public long? Timeout { get; set; }
    public string ParametersFilePath { get; set; }
    public bool AttachRobotLogs { get; set; }
    
    public string? RepositoryUrl { get; set; }
    public string? RepositoryCommit { get; set; }
    public string? RepositoryBranch { get; set; }
    public string? RepositoryType { get; set; }
    public string? ProjectUrl { get; set; }
    public bool DisableBuiltInNugetFeeds { get; set; }
    public string? ReleaseNotes { get; set; }

    public override string GetInlineCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"test run \"{OrchestratorUrl}\" \"{OrchestratorTenant}\"");
        if (ProjectPath is not null)
            commandArgs.Append($" --project-path \"{ProjectPath}\"");
        if (TestSet is not null)
            commandArgs.Append($" --testset \"{TestSet}\"");
        if (TestReportType is not null)
            commandArgs.Append($" --out \"{TestReportType}\"");
        if (TestReportDestination is not null)
            commandArgs.Append($" --result_path \"{TestReportDestination}\"");
        if (Environment is not null)
            commandArgs.Append($" --environment \"{Environment}\"");
        if (RetryCount is not null)
            commandArgs.Append($" --retryCount \"{RetryCount}\"");
        if (Timeout is not null)
            commandArgs.Append($" --timeout \"{Timeout}\"");
        if (ParametersFilePath is not null)
            commandArgs.Append($" --input_path \"{ParametersFilePath}\"");
        if (AttachRobotLogs)
            commandArgs.Append($" --attachRobotLogs true");
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
            commandArgs.Append($" --username \"{Username}\"");
        if (Password is not null)
            commandArgs.Append($" --password \"{Password}\"");
        if (RefreshToken is not null)
            commandArgs.Append($" --token \"{RefreshToken}\"");
        if (AccountName is not null)
            commandArgs.Append($" --accountName \"{AccountName}\"");
        if (AccountForApp is not null)
            commandArgs.Append($" --accountForApp \"{AccountForApp}\"");
        if (ApplicationId is not null)
            commandArgs.Append($" --applicationId \"{ApplicationId}\"");
        if (ApplicationSecret is not null)
            commandArgs.Append($" --applicationSecret \"{ApplicationSecret}\"");
        if (ApplicationScope is not null)
            commandArgs.Append($" --applicationScope \"{ApplicationScope}\"");
        if (OrganizationUnit is not null)
            commandArgs.Append($" --organizationUnit \"{OrganizationUnit}\"");
        if (Language is not null)
            commandArgs.Append($" --language \"{Language}\"");
        if (DisableTelemetry)
            commandArgs.Append($" --disableTelemetry");
        if (TraceLevel is not null)
            commandArgs.Append($" --traceLevel \"{TraceLevel}\"");
        if (AuthorizationUrl is not null)
            commandArgs.Append($" --identityUrl \"{AuthorizationUrl}\"");

        return commandArgs.ToString();
    }

    public override string GetInlineShortCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"test run \"{OrchestratorUrl}\" \"{OrchestratorTenant}\"");
        if (ProjectPath is not null)
            commandArgs.Append($" --P \"{ProjectPath}\"");
        if (TestSet is not null)
            commandArgs.Append($" --s \"{TestSet}\"");
        if (TestReportType is not null)
            commandArgs.Append($" --out \"{TestReportType}\"");
        if (TestReportDestination is not null)
            commandArgs.Append($" --r \"{TestReportDestination}\"");
        if (Environment is not null)
            commandArgs.Append($" --e \"{Environment}\"");
        if (RetryCount is not null)
            commandArgs.Append($" --retryCount \"{RetryCount}\"");
        if (Timeout is not null)
            commandArgs.Append($" --w \"{Timeout}\"");
        if (ParametersFilePath is not null)
            commandArgs.Append($" --i \"{ParametersFilePath}\"");
        if (AttachRobotLogs)
            commandArgs.Append($" --attachRobotLogs true");
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
            commandArgs.Append($" --applicationScope \"{ApplicationScope}\"");
        if (OrganizationUnit is not null)
            commandArgs.Append($" -o \"{OrganizationUnit}\"");
        if (Language is not null)
            commandArgs.Append($" -l \"{Language}\"");
        if (DisableTelemetry)
            commandArgs.Append($" -y");
        if (TraceLevel is not null)
            commandArgs.Append($" --traceLevel \"{TraceLevel}\"");
        if (AuthorizationUrl is not null)
            commandArgs.Append($" --identityUrl \"{AuthorizationUrl}\"");

        return commandArgs.ToString();
    }
}
