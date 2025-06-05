using System.Diagnostics;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class OrchestratorConnectionShould
{
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    // [UserPassModernFolderConnections]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task BeUsedToFetchLibraryFromOrchestratorTenantFeed(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.ProjectWithCustomLibraryFromOrchestrator_CrossPlatform_VB);
        var cachedPackages = new string[] { "library_crossplatform_vb", "library_crossplatform_vb.library.runtime" };

        foreach (var cachedPackage in cachedPackages)
        {
            Common.Utils.DeleteCachedPackage(cachedPackage);
        }
        await cliExecutor.Analyze(projectJsonPath, expectedExitCode: CliExitCode.Failure, analyzerTraceLevel: TraceLevel.Warning);
        await cliExecutor.Analyze(projectJsonPath, connection: connection, analyzerTraceLevel: TraceLevel.Warning);

        foreach (var cachedPackage in cachedPackages)
        { 
            Common.Utils.DeleteCachedPackage(cachedPackage);
        }
        await cliExecutor.Pack(projectJsonPath, expectedExitCode: CliExitCode.Failure);
        await cliExecutor.Pack(projectJsonPath, connection: connection);

        foreach (var cachedPackage in cachedPackages)
        {
            Common.Utils.DeleteCachedPackage(cachedPackage);
        }
        var result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectJsonPath, expectedExitCode: CliExitCode.Failure);
        Assert.Contains(result.OutputLines, line => line.Contains("packages/library_crossplatform_vb/1.0.2/library_crossplatform_vb.1.0.2.nupkg"));
    }
}
