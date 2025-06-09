package com.clie2e.examples;

public class ExampleContentProvider {
    
    public static String getContent(ExampleCategory category) {
        switch (category) {
            case ANALYZE:
                return getAnalyzeExample();
            case DEPLOY:
                return getDeployExample();
            case MANAGE_ASSETS:
                return getManageAssetsExample();
            case PACK:
                return getPackExample();
            case RUN_JOB:
                return getRunJobExample();
            case RUN_TESTS:
                return getRunTestsExample();
            default:
                return "// No examples available for this category";
        }
    }

    private static String getAnalyzeExample() {
        return """
// CLI E2E Analyze Example

using System.Diagnostics;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Dtos;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class AnalyzeShould
{
    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task AnalyzeWindowsProject(CliExecutor cliExecutor)
    {
        const string analyzeSuccessLog = "Done analyzing project";
        const string analyzeErrorLog = "Done analyzing with errors.";

        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.Windows, Entries.VB, Entries.SimpleLog_Windows_VB);
        var ignoredRules = "ST-USG-034";
        var result = await cliExecutor.Analyze(projectJsonPath, ignoredRules: ignoredRules);
        Assert.Contains(result.OutputLines, line => line.Contains(analyzeSuccessLog));
        Assert.DoesNotContain(result.OutputLines, line => line.Contains(analyzeErrorLog));
    }
}
""";
    }

    private static String getDeployExample() {
        return """
// CLI E2E Deploy Example

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
}
""";
    }

    private static String getManageAssetsExample() {
        return """
// CLI E2E Manage Assets Example

using System.Text;
using Microsoft.Rest;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class ManageAssetsShould
{
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task CreateUpdateAndDeleteAssets(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        const string integerAssetType = "integer";
        const string credentialAssetType = "credential";
        const string booleanAssetType = "bool";
        const string textAssetType = "text";

        var integerAssetName = Guid.NewGuid().ToString();
        var integerAssetCreateValue = 1;
        var integerAssetUpdateValue = 11;

        var credentialAssetName = Guid.NewGuid().ToString();
        var credentialAssetCreateValue = "username1::password1";

        var booleanAssetName = Guid.NewGuid().ToString();
        var booleanAssetCreateValue = "true";
        
        var textAssetName = Guid.NewGuid().ToString();
        var textAssetCreateValue = "it works!";

        var firstDeployment = new Dictionary<string, (string, object)>
        {
            { integerAssetName, (integerAssetType, integerAssetCreateValue) },
            { credentialAssetName, (credentialAssetType, credentialAssetCreateValue) }
        };

        var secondDeployment = new Dictionary<string, (string, object)>
        {
            { integerAssetName, (integerAssetType, integerAssetUpdateValue) },
            { booleanAssetName, (booleanAssetType, booleanAssetCreateValue) },
            { textAssetName, (textAssetType, textAssetCreateValue) }
        };

        // Deploy first set of assets
        var deploymentFile = Utils.CreateDeployAssetsFile(firstDeployment);
        await cliExecutor.DeployAssets(connection, deploymentFile);

        // Deploy second set (update + new assets)
        deploymentFile = Utils.CreateDeployAssetsFile(secondDeployment);
        await cliExecutor.DeployAssets(connection, deploymentFile);

        // Delete all assets
        var assetsToDelete = $"{integerAssetName},{credentialAssetName},{booleanAssetName},{textAssetName}";
        await cliExecutor.DeleteAssets(connection, assetsToDelete);
    }
}
""";
    }

    private static String getPackExample() {
        return """
// CLI E2E Pack Example

using NuGet.Packaging;
using NuGet.Packaging.Core;
using UiPath.Extensions.CommandLine.E2E.Tests.Builders;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Dtos;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options.Enums;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class PackShould
{
    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [WindowsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task OutputAProcessPackageOnAnyPlatformGivenAProcess(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);
        
        var result = await cliExecutor.Pack(projectJsonPath, outputType: OutputType.Process);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB);
    }
    
    private static void AssertDestinationFolderContainsNupkgWithGivenMetadata(string outputFolder, string projectName)
    {
        var fileInDirectory = Directory.GetFiles(outputFolder).First(file => file.EndsWith(FileExtensions.NugetPackage));
        Assert.True(fileInDirectory.Contains(projectName));
        
        var packageArchiveReader = new PackageArchiveReader(fileInDirectory);
        var id = packageArchiveReader.GetIdentity();
        Assert.Equal(projectName, id.Id);
    }
}
""";
    }

    private static String getRunJobExample() {
        return """
// CLI E2E Run Job Example

using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class RunJobShould
{
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task ExecuteABasicProcessWithInputArguments(CliExecutor cliExecutor, OrchestratorConnection connection)
    {        
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.WithBoolParam_CrossPlatform_VB);
        var result = await cliExecutor.DeployAndCreateProcess(projectFolder, connection);

        var arguments = new Dictionary<string, object>
        {
            { "flag", true }
        };
        var inputArgsPath = CreateFileWithJobInputParams(arguments);

        var jobsCount = new Random().Next(2, 5);
        var resultFilePath = Common.Utils.GetRandomJsonFileInTempPath();

        await cliExecutor.RunJob(result.CreatedProcessName, connection, parametersFilePath: inputArgsPath, jobsCount: jobsCount, resultFilePath: resultFilePath);

        Common.Utils.AssertJobResults(resultFilePath, expectedNumberOfJobExecutions: jobsCount);
        
        var httpClient = connection.GetClientWithIdentityHandler();
        var jobsClient = new JobsClient(new(), httpClient);
        var filter = $"ReleaseName eq '{result.CreatedProcessName}'";
        var jobsResponse = await jobsClient.GetAsync(filter: filter);
        
        var jobs = jobsResponse.Body.Value;
        Assert.Equal(jobsCount, jobs.Count);
        Assert.All(jobs, job => Assert.Equal(JobState.Successful, job.State));
    }
    
    private static string CreateFileWithJobInputParams(Dictionary<string, object> arguments)
    {
        var parametersFile = Utils.GetRandomJsonFileInTempPath();
        Utils.WriteObjectAsJson(parametersFile, arguments);
        return parametersFile;
    }
}
""";
    }

    private static String getRunTestsExample() {
        return """
// CLI E2E Run Tests Example

using Microsoft.Rest;
using NuGet.Packaging;
using UiPath.Extensions.CommandLine.E2E.Tests.Builders;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Dtos.InputParam;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options.Enums;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class RunTestsShould
{
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task PackCreateAndRunTestSet(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.TestCase_CrossPlatform_VB);
        var inputParamsPath = CreateFileWithTestSetInputParams(paramValue: "false");
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;
        await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, parametersFilePath: inputParamsPath, environment: environment, testReportDestination: reportDestination, expectedExitCode: CliExitCode.Failure);
        var result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, environment: environment, testReportDestination: reportDestination);

        var httpClient = connection.GetClientWithIdentityHandler();
        var testSetExecutionsClient = new TestSetExecutionsClient(new(), httpClient);

        var filter = $"contains(Name,'{result.DeployedProjectName}')";
        var executionsResponse = await testSetExecutionsClient.GetAsync(filter: filter);

        var executions = executionsResponse.Body.Value;
        Assert.Equal(1, executions.Count);

        var testSetExecution = executions[0];
        Assert.Equal(TestSetExecutionStatus.Passed, testSetExecution.Status);

        Common.Utils.AssertTestReport(reportDestination, result.DeployedProjectName);
    }
    
    private static string CreateFileWithTestSetInputParams(string paramValue)
    {
        var inputParam = new TestSetInputParamBuilder()
            .WithName("param")
            .WithType(ParamType.Input)
            .WithValue(paramValue)
            .Build();
        var paramList = new[] { inputParam };
        var parametersFile = Utils.GetRandomJsonFileInTempPath();
        Utils.WriteObjectAsJson(parametersFile, paramList);
        return parametersFile;
    }
}
""";
    }
}