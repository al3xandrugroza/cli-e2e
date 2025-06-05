namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;

internal class DeployNupkgResult : CliExecutionResult
{
    public DeployNupkgResult(CliExecutionResult executionResult) : base(executionResult) { }
}
