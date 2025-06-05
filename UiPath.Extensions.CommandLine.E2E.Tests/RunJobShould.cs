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

    [Theory]
    [ModernFolderConnectionsWithRobotHostsExpanded]
    [ModernFolderConnectionsWithRobotHostsExpandedWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task ExecuteWithDynamicStrategy(CliExecutor cliExecutor, OrchestratorConnection connection, string hostMachineName, string machineUsername)
    {
        if (connection.IsClassicFolder)
            throw new Exception("This test should be executed only with connections to modern folders");

        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB);
        var result = await cliExecutor.DeployAndCreateProcess(projectFolder, connection);

        await cliExecutor.RunJob(result.CreatedProcessName, connection, machine: hostMachineName, user: machineUsername, jobType: RobotType.Unattended);

        var httpClient = connection.GetClientWithIdentityHandler();
        var jobsClient = new JobsClient(new(), httpClient);
        var filter = $"ReleaseName eq '{result.CreatedProcessName}'";
        var jobsResponse = await jobsClient.GetAsync(filter: filter, expand: "Robot");

        var jobs = jobsResponse.Body.Value;
        Assert.Equal(1, jobs.Count);
        Assert.Equal(JobState.Successful, jobs[0].State);
        Assert.Equal(machineUsername, jobs[0].Robot.Username);
        Assert.Equal(hostMachineName, jobs[0].HostMachineName);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    // [UserPassClassicFolderConnections]
    // [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task WaitForJobCompletion(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SleepGivenAmountOfTime_CrossPlatform_VB);
        var firstResult= await cliExecutor.DeployAndCreateProcess(projectFolder, connection);

        var secondEnvironment = new string[] { connection.ClassicFolderProductionEnvironments.ElementAt(1).EnvironmentName };
        var secondResult = await cliExecutor.DeployAndCreateProcess(projectFolder, connection, environments: secondEnvironment);

        await cliExecutor.RunJob(firstResult.CreatedProcessName, connection, waitForJobCompletion: false);
        var httpClient = connection.GetClientWithIdentityHandler();
        var jobsClient = new JobsClient(new(), httpClient);
        var filter = $"ReleaseName eq '{firstResult.CreatedProcessName}'";
        var orderBy = "CreationTime desc";
        await Task.Delay(5_000);
        var jobsResponse = await jobsClient.GetAsync(filter: filter, orderby: orderBy);

        var jobs = jobsResponse.Body.Value;
        Assert.Equal(1, jobs.Count);
        Assert.NotEqual(JobState.Successful, jobs[0].State);
        Assert.NotEqual(JobState.Faulted, jobs[0].State);
        Assert.NotEqual(JobState.Stopped, jobs[0].State);


        var arguments = new Dictionary<string, object>
        {
            { "seconds", 20 }
        };
        var inputArgsPath = CreateFileWithJobInputParams(arguments);
        await cliExecutor.RunJob(secondResult.CreatedProcessName, connection, parametersFilePath: inputArgsPath);

        filter = $"ReleaseName eq '{secondResult.CreatedProcessName}'";
        jobsResponse = await jobsClient.GetAsync(filter: filter, orderby: orderBy);
        jobs = jobsResponse.Body.Value;
        Assert.Equal(1, jobs.Count);
        Assert.Equal(JobState.Successful, jobs[0].State);

        filter = $"ReleaseName eq '{firstResult.CreatedProcessName}'";
        jobsResponse = await jobsClient.GetAsync(filter: filter, orderby: orderBy);
        jobs = jobsResponse.Body.Value;
        Assert.Equal(1, jobs.Count);
        Assert.NotEqual(JobState.Successful, jobs[0].State);
        Assert.NotEqual(JobState.Faulted, jobs[0].State);
        Assert.NotEqual(JobState.Stopped, jobs[0].State);
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task FailWhenJobFails(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.WithBoolParam_CrossPlatform_VB);
        var result = await cliExecutor.DeployAndCreateProcess(projectFolder, connection);

        var resultFilePath = Common.Utils.GetRandomJsonFileInTempPath();
        await cliExecutor.RunJob(result.CreatedProcessName, connection, resultFilePath: resultFilePath, expectedExitCode: CliExitCode.Failure);
        Common.Utils.AssertJobResults(resultFilePath, expectedStatus: "Faulted");

        resultFilePath = Common.Utils.GetRandomJsonFileInTempPath();
        await cliExecutor.RunJob(result.CreatedProcessName, connection, resultFilePath: resultFilePath, failWhenJobFails: false);
        Common.Utils.AssertJobResults(resultFilePath, expectedStatus: "Faulted");
    }

    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [UserPassClassicFolderConnections]
    [UserPassClassicFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task Timeout(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        var projectFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SleepGivenAmountOfTime_CrossPlatform_VB);
        var result = await cliExecutor.DeployAndCreateProcess(projectFolder, connection);

        var arguments = new Dictionary<string, object>
        {
            { "seconds", 20 }
        };
        var inputArgsPath = CreateFileWithJobInputParams(arguments);
        await cliExecutor.RunJob(result.CreatedProcessName, connection, parametersFilePath: inputArgsPath);

        await cliExecutor.RunJob(result.CreatedProcessName, connection, timeout: 1, expectedExitCode: CliExitCode.Failure);
    }

    private static string CreateFileWithJobInputParams(Dictionary<string, object> arguments)
    {
        var argumentsFilePath = Common.Utils.GetRandomJsonFileInTempPath();
        Common.Utils.WriteObjectAsJson(argumentsFilePath, arguments);

        return argumentsFilePath;
    }
}
