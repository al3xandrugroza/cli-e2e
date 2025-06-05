namespace UiPath.Extensions.CommandLine.E2E.Tests.Exceptions;

internal class EnvironmentVariableNotDefinedException : Exception
{
    public EnvironmentVariableNotDefinedException(string message) : base(message) { }
}
