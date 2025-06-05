using System.Reflection;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using Xunit.Sdk;
using Connections = UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections.OrchestratorConnections;

namespace UiPath.Extensions.CommandLine.E2E.Tests.TestData;

internal class UserPassModernFolderConnectionsWithInlineArgsCliAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var success = bool.TryParse(Environment.GetEnvironmentVariable(AgentEnvironment.AcsDemo) ?? "False", out var acsDemo);
        if (!success) acsDemo = false;

        if (acsDemo) yield break;
        var tools = new List<CliExecutor>
        {
            new(CliCompatibility.CrossPlatform, CommandType.VerboseInline),
            new(CliCompatibility.CrossPlatform, CommandType.ShortInline)
        };

        foreach (var cli in tools)
            yield return [cli, Connections.UserPassModernFolderPaas_23_4];
    }
}
