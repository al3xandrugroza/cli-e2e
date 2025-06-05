using System.Text;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;

internal class RunJobOptions : TaskOptions
{
    public string ProcessName { get; set; }
    public string ParametersFilePath { get; set; }

    public string Machine { get; set; }
    public string User { get; set; }
    public IEnumerable<string> Robots { get; set; }

    public RobotType? JobType { get; set; }
    public long? JobsCount { get; set; }
    public JobPriority? Priority { get; set; }

    public bool? FailWhenJobFails { get; set; }
    public bool? WaitForJobCompletion { get; set; }
    public long? Timeout { get; set; }

    public string ResultFilePath { get; set; }

    public override string GetInlineCommandArgs()
    {
        var commandArgs = new StringBuilder();
        commandArgs.Append($"job run \"{ProcessName}\" \"{OrchestratorUrl}\" \"{OrchestratorTenant}\"");
        if (ParametersFilePath is not null)
            commandArgs.Append($" --input_path \"{ParametersFilePath}\"");
        if (Priority is not null)
            commandArgs.Append($" --priority \"{Priority}\"");
        if (Robots is not null)
            commandArgs.Append($" --robots \"{string.Join(",", Robots)}\"");
        if (JobsCount is not null)
            commandArgs.Append($" --jobscount \"{JobsCount}\"");
        if (User is not null)
            commandArgs.Append($" --user \"{User}\"");
        if (Machine is not null)
            commandArgs.Append($" --machine \"{Machine}\"");
        if (ResultFilePath is not null)
            commandArgs.Append($" --result_path \"{ResultFilePath}\"");
        if (Timeout is not null)
            commandArgs.Append($" --timeout \"{Timeout}\"");
        if (FailWhenJobFails == false)
            commandArgs.Append($" --fail_when_job_fails false");
        else if (FailWhenJobFails == true)
            commandArgs.Append($" --fail_when_job_fails true");
        if (WaitForJobCompletion == false)
            commandArgs.Append($" --wait false");
        else if (WaitForJobCompletion == true)
            commandArgs.Append($" --wait true");
        if (JobType is not null)
            commandArgs.Append($" --job_type \"{JobType}\"");
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
        commandArgs.Append($"job run \"{ProcessName}\" \"{OrchestratorUrl}\" \"{OrchestratorTenant}\"");
        if (ParametersFilePath is not null)
            commandArgs.Append($" -i \"{ParametersFilePath}\"");
        if (Priority is not null)
            commandArgs.Append($" -P \"{Priority}\"");
        if (Robots is not null)
            commandArgs.Append($" -r \"{string.Join(",", Robots)}\"");
        if (JobsCount is not null)
            commandArgs.Append($" -j \"{JobsCount}\"");
        if (User is not null)
            commandArgs.Append($" -U \"{User}\"");
        if (Machine is not null)
            commandArgs.Append($" -M \"{Machine}\"");
        if (ResultFilePath is not null)
            commandArgs.Append($" -R \"{ResultFilePath}\"");
        if (Timeout is not null)
            commandArgs.Append($" -W \"{Timeout}\"");
        if (FailWhenJobFails == false)
            commandArgs.Append($" -f false");
        else if (FailWhenJobFails == true)
            commandArgs.Append($" -f true");
        if (WaitForJobCompletion == false)
            commandArgs.Append($" -w false");
        else if (WaitForJobCompletion == true)
            commandArgs.Append($" -w true");
        if (JobType is not null)
            commandArgs.Append($" -b \"{JobType}\"");
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
