using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class DeployShould
{
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    // [UserPassClassicFolderConnections]
    // [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task DeployAProcessPackageOnFeed(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB);
        var environments = connection.IsClassicFolder ? new string[] { connection.ClassicFolderTestingEnvironments.First().EnvironmentName } : null;
        var result = await cliExecutor.Deploy(projectFolder, connection, environments: environments);

        var httpClient = connection.GetClientWithIdentityHandler();
        var packagesClient = new ProcessesClient(new(), httpClient);

        var filter = $"Id eq '{result.DeployedProjectName}'";
        var packagesResponse = await packagesClient.GetAsync(feedId: connection.FolderFeedId, filter: filter);

        Assert.Equal(1, packagesResponse.Body.Value.Count);
    }
    
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    // [UserPassClassicFolderConnections]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task DeployALibraryPackageOnFeed(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.Library_CrossPlatform_VB);
        var result = await cliExecutor.Deploy(projectFolder, connection);

        var httpClient = connection.GetClientWithIdentityHandler();
        var librariesClient = new LibrariesClient(new(), httpClient);

        var filter = $"Id eq '{result.DeployedProjectName}'";
        var packagesResponse = await librariesClient.GetAsync(filter: filter);

        Assert.Equal(1, packagesResponse.Body.Value.Count);
    }
    
    [Theory]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    // [UserPassClassicFolderConnections]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task IgnoreLibraryDeployConflict(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.Library_CrossPlatform_VB);
        var version = $"1.0.{new Random().Next()}";
        var result = await cliExecutor.Deploy(projectFolder, connection, autoVersion: false, version: version);
        await cliExecutor.Deploy(projectFolder, connection, autoVersion: false, version: version, overwriteName: result.DeployedProjectName, expectedExitCode: CliExitCode.Failure);
        await cliExecutor.Deploy(projectFolder, connection, autoVersion: false, version: version, overwriteName: result.DeployedProjectName, ignoreLibraryDeployConflict: true);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task DeployCreateAndUpdateProcess(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        if (connection.IsClassicFolder)
            throw new Exception("This test is designed for modern folder connections only");

        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB);
        var result = await cliExecutor.DeployAndCreateProcess(projectFolder, connection);

        var httpClient = connection.GetClientWithIdentityHandler();
        var processesClient = new ReleasesClient(new(), httpClient);
        var packagesClient = new ProcessesClient(new(), httpClient);

        var createdProcesses = new string[] { result.CreatedProcessName };
        await AssertProcessesInSyncWithSourcePacakge(connection, packagesClient, processesClient, result.DeployedProjectName, createdProcesses);


        await cliExecutor.Deploy(projectFolder, connection, createProcess: true, overwriteName: result.DeployedProjectName);
        await AssertProcessesInSyncWithSourcePacakge(connection, packagesClient, processesClient, result.DeployedProjectName, createdProcesses);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task CreateProcessWithMultipleEntryPoints(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        if (connection.IsClassicFolder)
            throw new Exception("This test is designed for modern folder connections only");

        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.MultipleEntryPoints_CrossPlatform_VB);
        var entryPointPaths = new List<string> { Entries.SuccessEntryPoint, Entries.FailEntryPoint };
        var result = await cliExecutor.Deploy(projectFolder, connection, createProcess: true, entryPointPaths: entryPointPaths);
        var successfulProcessName = $"{result.DeployedProjectName}_{Entries.SuccessEntryPoint}";
        var unsuccessfulProcessName = $"{result.DeployedProjectName}_{Entries.FailEntryPoint}";

        var httpClient = connection.GetClientWithIdentityHandler();
        var processesClient = new ReleasesClient(new NSwagClientConfig(), httpClient);
        var packagesClient = new ProcessesClient(new NSwagClientConfig(), httpClient);

        var createdProcesses = new string[] { successfulProcessName, unsuccessfulProcessName };
        await AssertProcessesInSyncWithSourcePacakge(connection, packagesClient, processesClient, result.DeployedProjectName, createdProcesses);


        await cliExecutor.Deploy(projectFolder, connection, createProcess: true, entryPointPaths: entryPointPaths, overwriteName: result.DeployedProjectName);
        await AssertProcessesInSyncWithSourcePacakge(connection, packagesClient, processesClient, result.DeployedProjectName, createdProcesses);

        await cliExecutor.RunJob(successfulProcessName, connection);
        await cliExecutor.RunJob(unsuccessfulProcessName, connection, expectedExitCode: CliExitCode.Failure);
    }

    private static async Task AssertProcessesInSyncWithSourcePacakge(OrchestratorConnection connection, ProcessesClient packagesClient, ReleasesClient processesClient, string sourcePackageName, IEnumerable<string> processNames)
    {
        var packagesFilter = $"Id eq '{sourcePackageName}'";

        var wrappedNames = processNames.Select(name => $"'{name}'");
        var filterList = "(" + string.Join(",", wrappedNames) + ")";
        var processesFilter = $"Name in {filterList}";
        
        var packagesResponse = await packagesClient.GetAsync(feedId: connection.FolderFeedId, filter: packagesFilter);
        var processesResponse = await processesClient.GetAsync(filter: processesFilter);
        var package = packagesResponse.Body.Value[0];
        var processes = processesResponse.Body.Value;
        
        Assert.Equal(processNames.Count(), processes.Count);
        foreach(var process in processes)
        {
            Assert.Equal(package.Version, process.ProcessVersion);
            Assert.Equal(sourcePackageName, process.ProcessKey.ToString());
        }
    }
}
