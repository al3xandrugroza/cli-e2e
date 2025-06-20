﻿using System.Text;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;

internal class DeployOptions : TaskOptions
{
    public string? PackagesPath { get; set; }
    public bool? CreateProcess { get; set; }
    public bool IgnoreLibraryDeployConflict { get; set; }
    public IEnumerable<string> EntryPointPaths { get; set; }
    public IEnumerable<string> Environments { get; set; }

    public override string GetInlineCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"package deploy \"{PackagesPath}\" \"{OrchestratorUrl}\" \"{OrchestratorTenant}\"");
        if (CreateProcess == false)
            commandArgs.Append($" --createProcess false");
        else if (CreateProcess == true)
            commandArgs.Append($" --createProcess true");
        if (IgnoreLibraryDeployConflict)
            commandArgs.Append($" --ignoreLibraryDeployConflict");
        if (Environments is not null)
            commandArgs.Append($" --environments \"{string.Join(",", Environments)}\"");
        if (EntryPointPaths is not null)
            commandArgs.Append($" --entryPointsPath \"{string.Join(",", EntryPointPaths)}\"");
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
        commandArgs.Append($"package deploy \"{PackagesPath}\" \"{OrchestratorUrl}\" \"{OrchestratorTenant}\"");
        if (CreateProcess == false)
            commandArgs.Append($" -c false");
        else if (CreateProcess == true)
            commandArgs.Append($" -c true");
        if (IgnoreLibraryDeployConflict)
            commandArgs.Append($" --ignoreLibraryDeployConflict");
        if (Environments is not null)
            commandArgs.Append($" -e \"{string.Join(",", Environments)}\"");
        if (EntryPointPaths is not null)
            commandArgs.Append($" -h \"{string.Join(",", EntryPointPaths)}\"");
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
