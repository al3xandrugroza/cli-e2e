namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;

internal class DeployResult : CliExecutionResult
{
    public DeployResult(CliExecutionResult executionResult) : base(executionResult) { }

    public DeployResult(DeployResult executionResult) : base(executionResult)
    {
        DeployedProjectName = executionResult.DeployedProjectName;
    }

    public string DeployedProjectName { get; set; }
}
