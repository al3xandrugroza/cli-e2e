namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;

internal class PackResult : CliExecutionResult
{
    public PackResult(CliExecutionResult executionResult) : base(executionResult) { }
    
    public string OutputFolder { get; set; }
}
