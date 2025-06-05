using System.Reflection;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using Xunit.Sdk;
using Connections = UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections.OrchestratorConnections;

namespace UiPath.Extensions.CommandLine.E2E.Tests.TestData;

internal class ExtAppModernFolderConnectionsWithInlineArgsCliAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var tools = new List<CliExecutor>
        {
            new(CliCompatibility.CrossPlatform, CommandType.VerboseInline),
            new(CliCompatibility.CrossPlatform, CommandType.ShortInline)
        };

        foreach (var cli in tools)
            yield return [cli, Connections.ExtAppModernFolderCloud];
    }
}
