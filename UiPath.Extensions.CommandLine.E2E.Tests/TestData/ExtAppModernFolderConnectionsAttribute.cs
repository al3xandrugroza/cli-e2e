using System.Reflection;
using System.Runtime.InteropServices;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using Xunit.Sdk;
using Connections = UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections.OrchestratorConnections;

namespace UiPath.Extensions.CommandLine.E2E.Tests.TestData;

internal class ExtAppModernFolderConnectionsAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var success = bool.TryParse(Environment.GetEnvironmentVariable(AgentEnvironment.AcsDemo) ?? "False", out var acsDemo);
        if (!success) acsDemo = false;
        
        var tools = new List<CliExecutor> { new(CliCompatibility.CrossPlatform) };
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            tools.Add(new CliExecutor(CliCompatibility.Windows));

        foreach (var cli in tools)
        {
            if (!acsDemo)
            {
                yield return [cli, Connections.ExtAppModernFolderPaas_21_10];
                yield return [cli, Connections.ExtAppModernFolderPaas_22_4];
                yield return [cli, Connections.ExtAppModernFolderPaas_22_10];
                yield return [cli, Connections.ExtAppModernFolderPaas_23_4];
            }
            
            yield return [cli, Connections.ExtAppModernFolderCloud];
        }
    }
}
