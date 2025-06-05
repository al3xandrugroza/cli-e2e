namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;

public class CliExecutionResult
{
    public CliExitCode ExitCode { get; set; }
    public List<string> OutputLines { get; }
    public List<string> ErrorLines { get; }

    public CliExecutionResult()
    {
        OutputLines = new List<string>();
        ErrorLines = new List<string>();
    }

    protected CliExecutionResult(CliExecutionResult result)
    {
        ExitCode = result.ExitCode;
        OutputLines = result.OutputLines;
        ErrorLines = result.ErrorLines;
    }
}
