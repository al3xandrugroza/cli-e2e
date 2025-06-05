namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;

internal class RunTestsResult : CliExecutionResult
{
    public RunTestsResult(CliExecutionResult executionResult) : base(executionResult) { }

    public string DeployedProjectName { get; set; }
}
