using System.Reflection;
using System.Runtime.InteropServices;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using Xunit.Sdk;
using Connections = UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections.OrchestratorConnections;

namespace UiPath.Extensions.CommandLine.E2E.Tests.TestData;

internal class ModernFolderConnectionsWithRobotHostsExpandedAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var success = bool.TryParse(Environment.GetEnvironmentVariable(AgentEnvironment.AcsDemo) ?? "False", out var acsDemo);
        if (!success) acsDemo = false;
        
        var tools = new List<CliExecutor> { new CliExecutor(CliCompatibility.CrossPlatform) };
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            tools.Add(new CliExecutor(CliCompatibility.Windows));

        foreach (var cli in tools)
        {
            if (!acsDemo)
            {
                foreach (var robotHost in Connections.ExtAppModernFolderPaas_21_10.ModernFolderRobotHosts)
                    yield return [ cli, Connections.ExtAppModernFolderPaas_21_10, robotHost.MachineHostname, robotHost.MachineUsername ];

                foreach (var robotHost in Connections.ExtAppModernFolderPaas_22_4.ModernFolderRobotHosts)
                    yield return [ cli, Connections.ExtAppModernFolderPaas_22_4, robotHost.MachineHostname, robotHost.MachineUsername ];

                foreach (var robotHost in Connections.ExtAppModernFolderPaas_22_10.ModernFolderRobotHosts)
                    yield return [ cli, Connections.ExtAppModernFolderPaas_22_10, robotHost.MachineHostname, robotHost.MachineUsername ];

                foreach (var robotHost in Connections.ExtAppModernFolderPaas_23_4.ModernFolderRobotHosts)
                    yield return [ cli, Connections.ExtAppModernFolderPaas_23_4, robotHost.MachineHostname, robotHost.MachineUsername ];
            }
            
            foreach (var robotHost in Connections.ExtAppModernFolderCloud.ModernFolderRobotHosts)
                yield return [cli, Connections.ExtAppModernFolderCloud, robotHost.MachineHostname, robotHost.MachineUsername];
        }
    }
}
