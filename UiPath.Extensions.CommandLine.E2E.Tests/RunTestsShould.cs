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

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    // [UserPassClassicFolderConnections]
    // [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task RunExistingTestSuite(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var httpClient = connection.GetClientWithIdentityHandler();
        var testSetsClient = new TestSetsClient(new(), httpClient);

        const string packageIdentifier = Entries.TestCase_CrossPlatform_VB + "_Tests_Tests";

        var testSetWithArgsName = $"{packageIdentifier}_with_args_{Guid.NewGuid()}";
        var testSetWithArgsBuilder = TestSetBuilder.Init()
                                                   .WithName(testSetWithArgsName)
                                                   .WithPackageIdentifier(packageIdentifier, connection.SuccessfulTestSetPackageVersion, connection.SuccessfulTestCaseDefinitionId, connection.SuccessfulTestCaseReleaseId)
                                                   .WithInputArgs();
        
        var testSetWithoutArgsName = $"{packageIdentifier}_without_args_{Guid.NewGuid()}";
        var testSetWithoutArgsBuilder = TestSetBuilder.Init()
            .WithName(testSetWithoutArgsName)
            .WithPackageIdentifier(packageIdentifier, connection.SuccessfulTestSetPackageVersion, connection.SuccessfulTestCaseDefinitionId, connection.SuccessfulTestCaseReleaseId);

        if (connection.IsClassicFolder)
        {
            testSetWithArgsBuilder.WithEnvironment(connection.SuccessfulTestSetEnvironmentId);
            testSetWithoutArgsBuilder.WithEnvironment(connection.SuccessfulTestSetEnvironmentId);
        }

        var testSetWithArgsDto = testSetWithArgsBuilder.Build();
        var testSetWithoutArgsDto = testSetWithoutArgsBuilder.Build();

        await Common.Utils.ExecuteIgnoreExceptionAsync<HttpOperationException>(task: async () => await testSetsClient.PostAsync(testSetWithArgsDto));
        await Common.Utils.ExecuteIgnoreExceptionAsync<HttpOperationException>(task: async () => await testSetsClient.PostAsync(testSetWithoutArgsDto));

        var inputParamsPath = CreateFileWithTestSetInputParams();
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;
        
        await cliExecutor.RunTests(connection, testSet: testSetWithArgsName, testReportDestination: reportDestination, environment: environment, expectedExitCode: CliExitCode.Failure);
        await cliExecutor.RunTests(connection, testSet: testSetWithArgsName, testReportDestination: reportDestination, parametersFilePath: inputParamsPath, environment: environment);
        Common.Utils.AssertTestReport(reportDestination, testSetWithArgsName);
        
        await cliExecutor.RunTests(connection, testSet: testSetWithoutArgsName, testReportDestination: reportDestination, environment: environment);
        Common.Utils.AssertTestReport(reportDestination, testSetWithoutArgsName);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task WriteUipathReportType(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.TestCase_CrossPlatform_VB);
        var inputParamsPath = CreateFileWithTestSetInputParams();
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        const TestReportType testReportType = TestReportType.uipath;
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;

        var result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, parametersFilePath: inputParamsPath, environment: environment, testReportType: testReportType,  testReportDestination: reportDestination);

        Common.Utils.AssertTestReport(reportDestination, result.DeployedProjectName, reportType: testReportType);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task WriteJunitReportType(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.TestCase_CrossPlatform_VB);
        var inputParamsPath = CreateFileWithTestSetInputParams();
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        const TestReportType testReportType = TestReportType.junit;
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;

        var result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, parametersFilePath: inputParamsPath, environment: environment, testReportType: testReportType, testReportDestination: reportDestination);

        Common.Utils.AssertTestReport(reportDestination, result.DeployedProjectName, reportType: testReportType);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task FailWhenTestSuiteFails(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.FailingTestCase_CrossPlatform_VB);
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;

        var result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, testReportDestination: reportDestination, environment: environment, expectedExitCode: CliExitCode.Failure);
        Common.Utils.AssertTestReport(reportDestination, result.DeployedProjectName, expectedNumberOfErrors: 1);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task Timeout(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.TestCaseWithDelay_CrossPlatform_VB);
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;

        await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, testReportDestination: reportDestination, environment: environment);
        await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, testReportDestination: reportDestination, environment: environment, timeout: 1, expectedExitCode: CliExitCode.Failure);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task AttachRobotLogs(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.TestCase_CrossPlatform_VB);
        var inputParamsPath = CreateFileWithTestSetInputParams();
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        
        await RunAndAssertRobotLogsAttachment(cliExecutor, connection, projectFolder, reportDestination, inputParamsPath, TestReportType.uipath);
        await RunAndAssertRobotLogsAttachment(cliExecutor, connection, projectFolder, reportDestination, inputParamsPath, TestReportType.junit);
    }
    
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task RunTestsWithRepositoryMetadata(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.TestCaseWithDelay_CrossPlatform_VB);
        var reportDestination = Common.Utils.GetRandomXmlFileInTempPath();
        
        var packageMetadata = PackageMetadataBuilder.Init().APackageMetadata().Build();

        var result = await cliExecutor.RunTests(connection, repositoryUrl: packageMetadata.RepositoryUrl, repositoryCommit: packageMetadata.RepositoryCommit, repositoryBranch: packageMetadata.RepositoryBranch, repositoryType: packageMetadata.RepositoryType, projectUrl: packageMetadata.ProjectUrl,
                                   sourceProjectFolder: projectFolder, testReportDestination: reportDestination, releaseNotes: packageMetadata.ReleaseNotes);

        var downloadedPackageStream = await DownloadPackageWithTestsFromFeed(connection, result.DeployedProjectName);
        var packageReader = new PackageArchiveReader(downloadedPackageStream);
        Common.Utils.ReadAndAssertPackageMetadata(packageReader, packageMetadata);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task DisableHardcodedFeeds(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.ProjectWithNonExistingCustomLibraryFromOrchestrator_CrossPlatform_VB);
        var cachedPackages = new string[] { "library_crossplatform_vb", "library_crossplatform_vb.library.runtime" };
        var builtInFeed = "https://pkgs.dev.azure.com/uipath/Public.Feeds/_packaging/UiPath-Official/nuget/v3/index.json";

        foreach (var cachedPackage in cachedPackages)
        {
            Common.Utils.DeleteCachedPackage(cachedPackage);
        }
        var result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectJsonPath, expectedExitCode: CliExitCode.Failure);
        Assert.Contains(result.OutputLines, line => line.Contains(builtInFeed));
        result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectJsonPath, expectedExitCode: CliExitCode.Failure, disableBuiltInNugetFeeds: true);
        Assert.DoesNotContain(result.OutputLines, line => line.Contains(builtInFeed));
    }
    
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    // [UserPassClassicFolderConnections]
    // [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task RetryFailedTestSet(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var httpClient = connection.GetClientWithIdentityHandler();
        var testSetsClient = new TestSetsClient(new(), httpClient);
        var testSetExecutionsClient = new TestSetExecutionsClient(new(), httpClient);
        var testCaseExecutionsClient = new TestCaseExecutionsClient(new(), httpClient);
        
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;

        const int retryCount = 3;
        const string packageIdentifier = $"{Entries.FailingTestCase_CrossPlatform_VB}_Tests";
        var newTestSet = $"{packageIdentifier}_{Guid.NewGuid()}";
        var newTestSetBuilder = TestSetBuilder.Init()
                                              .WithName(newTestSet)
                                              .WithPackageIdentifier(packageIdentifier, connection.FailingTestSetPackageVersion, connection.FailingTestCaseDefinitionId, connection.FailingTestCaseReleaseId);
        if (connection.IsClassicFolder) newTestSetBuilder.WithEnvironment(connection.FailingTestSetEnvironmentId);
        var newTestSetDto = newTestSetBuilder.Build();
        await Common.Utils.ExecuteIgnoreExceptionAsync<HttpOperationException>(task: async () => await testSetsClient.PostAsync(newTestSetDto));
        
        var runTask = cliExecutor.RunTests(connection, testSet: newTestSet, environment: environment, expectedExitCode: CliExitCode.Failure, retryCount: retryCount);
        
        await Task.Delay(10000); // wait for the first execution to start and finish
        
        var testSetExecutionsFilter = $"contains(Name,'{newTestSet}')";
        var executionsResponse = await testSetExecutionsClient.GetAsync(filter: testSetExecutionsFilter);
        var testSetExecutions = executionsResponse.Body.Value;
        var testCaseFilter = $"TestSetExecutionId eq {testSetExecutions.FirstOrDefault()?.Id}";
        var firstTestCaseExecution = (await testCaseExecutionsClient.GetAsync(filter: testCaseFilter)).Body.Value;

        var result = await runTask;
        var secondTestCaseExecution = (await testCaseExecutionsClient.GetAsync(filter: testCaseFilter)).Body.Value;

        Assert.Contains(result.OutputLines, line => line.Contains($"#{retryCount}"));
        Assert.True(firstTestCaseExecution[0].StartTime < secondTestCaseExecution[0].StartTime);
    }

    private static async Task<Stream?> DownloadPackageWithTestsFromFeed(OrchestratorConnection connection, string projectName)
    {
        var httpClient = connection.GetClientWithIdentityHandler();
        var packagesClient = new ProcessesClient(new(), httpClient);

        var filter = $"Id eq '{projectName}_Tests'";
        var packagesResponse = await packagesClient.GetAsync(feedId: connection.FolderFeedId, filter: filter);
        var package = packagesResponse.Body.Value[0];

        var downloadedPackage = await packagesClient.DownloadPackageAsync(package.Key, feedId: connection.FolderFeedId);
        return downloadedPackage?.Stream;
    }

    private static async Task RunAndAssertRobotLogsAttachment(CliExecutor cliExecutor, OrchestratorConnection connection, string projectFolder, string reportDestination, string inputParamsPath, TestReportType reportType)
    {
        var environment = connection.IsClassicFolder ? connection.ClassicFolderTestingEnvironments.First().EnvironmentName : null;
        var result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, parametersFilePath: inputParamsPath, testReportType: reportType, testReportDestination: reportDestination, environment: environment);
        Common.Utils.AssertTestReport(reportDestination, result.DeployedProjectName, reportType: reportType);

        result = await cliExecutor.RunTests(connection, sourceProjectFolder: projectFolder, parametersFilePath: inputParamsPath, testReportType: reportType,
            testReportDestination: reportDestination, attachRobotLogs: true, environment: environment);
        Common.Utils.AssertTestReport(reportDestination, result.DeployedProjectName, reportType: reportType, robotLogsAttachment: true);
    }

    private static string CreateFileWithTestSetInputParams(string paramValue = "true")
    {
        var testSetInputParams = new[] { 
            TestSetInputParamBuilder.Init()
            .WithName("flag")
            .WithType(ParamType.Bool)
            .WithValue(paramValue)
            .Build()
        };

        var testSetInputParamsPath = Common.Utils.GetRandomJsonFileInTempPath();
        Common.Utils.WriteObjectAsJson(testSetInputParamsPath, testSetInputParams);

        return testSetInputParamsPath;
    }
}
