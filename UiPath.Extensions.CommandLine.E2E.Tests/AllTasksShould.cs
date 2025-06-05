using System.Diagnostics;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;
using UiPath.Extensions.CommandLine.E2E.Tests.Utils;
using Connections = UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections.OrchestratorConnections;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class AllTasksShould
{
    [Theory]
    [CrossPlatformCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task CollectDataBasedOnTheEnvironment(CliExecutor cliExecutor)
    {
        var connection = Connections.ExtAppModernFolderCloud;

        Environment.SetEnvironmentVariable(EnvironmentVariables.TelemetryIsEnabled, "True");
        await AssertDataCollectionLog(cliExecutor, connection, Assert.Contains<string>);
        
        Environment.SetEnvironmentVariable(EnvironmentVariables.TelemetryIsEnabled, "False");
        await AssertDataCollectionLog(cliExecutor, connection, Assert.DoesNotContain<string>);

        Environment.SetEnvironmentVariable(EnvironmentVariables.TelemetryIsEnabled, "True");
    }

    private static async Task AssertDataCollectionLog(CliExecutor cliExecutor, OrchestratorConnection connection, Action<IEnumerable<string>, Predicate<string>> assertMethod, bool disableTelemetry = false)
    {
        const string telemetryEnabledLog = "uipcli: Data collection is enabled.";
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB);

        var result = await cliExecutor.Analyze(projectFolder, analyzerTraceLevel: TraceLevel.Warning, disableTelemetry: disableTelemetry);
        assertMethod(result.OutputLines, line => line.Contains(telemetryEnabledLog));

        result = await cliExecutor.Deploy(projectFolder, connection, disableTelemetry: disableTelemetry);
        assertMethod(result.OutputLines, line => line.Contains(telemetryEnabledLog));

        var integerAssetName = Guid.NewGuid().ToString();
        const string integerAssetType = "integer";
        var integerAssetCreateValue = 1;
        var assets = new Dictionary<string, (string, object)>
        {
            { integerAssetName, (integerAssetType, integerAssetCreateValue) },
        };
        var assetsFilePath = Common.Utils.CreateDeployAssetsFile(assets);
        result = await cliExecutor.ManageAssets<DeployAssetsOptions>(connection, assetsFilePath, disableTelemetry: disableTelemetry);
        assertMethod(result.OutputLines, line => line.Contains(telemetryEnabledLog));
        result = await cliExecutor.ManageAssets<DeleteAssetsOptions>(connection, assetsFilePath, disableTelemetry: disableTelemetry);
        assertMethod(result.OutputLines, line => line.Contains(telemetryEnabledLog));

        result = await cliExecutor.Pack(projectFolder);
        assertMethod(result.OutputLines, line => line.Contains(telemetryEnabledLog));

        projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.WithBoolParam_CrossPlatform_VB);
        var deployResult = await cliExecutor.Deploy(projectFolder, connection, createProcess: true);
        var createdProcessName = $"{deployResult.DeployedProjectName}_{Entries.DefaultEntryPoint}";
        result = await cliExecutor.RunJob(createdProcessName, connection, expectedExitCode: CliExitCode.Failure, disableTelemetry: disableTelemetry);
        assertMethod(result.OutputLines, line => line.Contains(telemetryEnabledLog));

        projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.FailingTestCase_CrossPlatform_VB);
        result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, expectedExitCode: CliExitCode.Failure, disableTelemetry: disableTelemetry);
        assertMethod(result.OutputLines, line => line.Contains(telemetryEnabledLog));
    }
}
