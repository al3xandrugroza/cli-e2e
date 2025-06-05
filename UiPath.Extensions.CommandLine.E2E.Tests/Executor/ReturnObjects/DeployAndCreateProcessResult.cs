namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;

internal class DeployAndCreateProcessResult : DeployResult
{
    public DeployAndCreateProcessResult(DeployResult executionResult) : base(executionResult) { }

    public string CreatedProcessName { get; set; }
}
