using System.Reflection;
using System.Runtime.InteropServices;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using Xunit.Sdk;
using Connections = UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections.OrchestratorConnections;

namespace UiPath.Extensions.CommandLine.E2E.Tests.TestData;

internal class ClassicFolderConnectionsWithProdEnvironmentsExpandedAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var success = bool.TryParse(Environment.GetEnvironmentVariable(AgentEnvironment.AcsDemo) ?? "False", out var acsDemo);
        if (!success) acsDemo = false;

        if (acsDemo) yield break;
        var tools = new List<CliExecutor> { new(CliCompatibility.CrossPlatform) };
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            tools.Add(new CliExecutor(CliCompatibility.Windows));

        foreach (var cli in tools)
        {
            foreach (var classicFolderEnvironment in Connections.UserPassClassicFolderPaas_21_10
                         .ClassicFolderProductionEnvironments)
                yield return new object[]
                {
                    cli, Connections.UserPassClassicFolderPaas_21_10, classicFolderEnvironment.EnvironmentName,
                    classicFolderEnvironment.RobotName
                };
        }
    }
}
