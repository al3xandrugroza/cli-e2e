using System.Diagnostics;
using Newtonsoft.Json.Linq;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options.Enums;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Extensions;

internal static class CliExecutorExtensions
{
    public static async Task<CliExecutionResult> Analyze(this CliExecutor cliExecutor, string projectJsonPath, OrchestratorConnection? connection = null, string ignoredRules = null, bool stopOnRuleViolation = true,
        bool treatWarningsAsErrors = false, bool populateWithConfigurationDetails = true, bool populateWithTelemetryData = true, TraceLevel? analyzerTraceLevel = null, string resultFilePath = null, CliExitCode expectedExitCode = CliExitCode.Success,
        TracingLevel? traceLevel = TracingLevel.Information, bool disableTelemetry = false, bool disableBuiltInNugetFeeds = false, string governanceFilePath = null)
    {
        var projectJsonPathClone = Common.Utils.GetDirectoryOfPath(projectJsonPath).CloneProjectIntoTempLocation();

        var options = new AnalyzeOptions
        {
            ProjectPath = projectJsonPathClone,
            IgnoredRules = ignoredRules,
            StopOnRuleViolation = stopOnRuleViolation,
            TreatWarningsAsErrors = treatWarningsAsErrors,
            AnalyzerTraceLevel = analyzerTraceLevel,
            ResultFilePath = resultFilePath,
            GovernanceFilePath = governanceFilePath,
            DisableBuiltInNugetFeeds = disableBuiltInNugetFeeds,
        }.PopulateWithConnectionDetails(connection);
        if (populateWithConfigurationDetails)
            options.PopulateWithConfigurationDetails(traceLevel: traceLevel, disableTelemetry: disableTelemetry);
        if (populateWithTelemetryData)
            options.PopulateWithTelemetryData();

        var analyzeOptions = new RunOptions
        {
            Type = typeof(AnalyzeOptions).Name,
            Options = options,
        };

        var executionResult = await cliExecutor.Execute(analyzeOptions);
        if (executionResult.ExitCode != expectedExitCode) executionResult.OutputDebugInformation();
        Assert.Equal(expectedExitCode, executionResult.ExitCode);

        return executionResult;
    }

    public static async Task<DeployResult> Deploy(this CliExecutor cliExecutor, string sourceProjectFolder, OrchestratorConnection connection, bool createProcess = false, IEnumerable<string>? entryPointPaths = null,
        bool populateWithConfigurationDetails = true, bool populateWithTelemetryData = true, IEnumerable<string> environments = null, string? overwriteName = null,
        TracingLevel? traceLevel = TracingLevel.Information, bool disableTelemetry = false, bool ignoreLibraryDeployConflict = false, CliExitCode expectedExitCode = CliExitCode.Success,
        bool autoVersion = true, string version = null)
    {
        var projectJsonPath = sourceProjectFolder.CloneProjectIntoTempLocation();
        var newUniqueProjectName = EnsureUniqueProjectName(projectJsonPath, uniqueName: overwriteName);

        var result = await cliExecutor.Pack(projectJsonPath, autoVersion: autoVersion, version: version);

        var options = new DeployOptions
        {
            PackagesPath = result.OutputFolder,
            CreateProcess = createProcess,
            IgnoreLibraryDeployConflict = ignoreLibraryDeployConflict,
            EntryPointPaths = entryPointPaths ?? new List<string> { Entries.DefaultEntryPoint },
            Environments = environments,
        }.PopulateWithConnectionDetails(connection);
        if (populateWithConfigurationDetails)
            options.PopulateWithConfigurationDetails(traceLevel: traceLevel, disableTelemetry: disableTelemetry);
        if (populateWithTelemetryData)
            options.PopulateWithTelemetryData();

        var deployOptions = new RunOptions
        {
            Type = typeof(DeployOptions).Name,
            Options = options,
        };

        var executionResult = await cliExecutor.Execute(deployOptions);
        if (executionResult.ExitCode != 0) executionResult.OutputDebugInformation();
        Assert.Equal(expectedExitCode, executionResult.ExitCode);

        return new DeployResult(executionResult)
        {
            DeployedProjectName = newUniqueProjectName,
        };
    }
    
    public static async Task<DeployNupkgResult> DeployNupkg(this CliExecutor cliExecutor, OrchestratorConnection connection, string packagePath, bool createProcess = false, IEnumerable<string>? entryPointPaths = null,
                                                            bool populateWithConfigurationDetails = true, bool populateWithTelemetryData = true, IEnumerable<string> environments = null,
                                                            TracingLevel? traceLevel = TracingLevel.Information, bool disableTelemetry = false, bool ignoreLibraryDeployConflict = false, CliExitCode expectedExitCode = CliExitCode.Success)
    {
        var options = new DeployOptions
        {
            PackagesPath = packagePath,
            CreateProcess = createProcess,
            IgnoreLibraryDeployConflict = ignoreLibraryDeployConflict,
            EntryPointPaths = entryPointPaths ?? new List<string> { Entries.DefaultEntryPoint },
            Environments = environments,
        }.PopulateWithConnectionDetails(connection);
        if (populateWithConfigurationDetails)
            options.PopulateWithConfigurationDetails(traceLevel: traceLevel, disableTelemetry: disableTelemetry);
        if (populateWithTelemetryData)
            options.PopulateWithTelemetryData();

        var deployOptions = new RunOptions
        {
            Type = typeof(DeployOptions).Name,
            Options = options,
        };

        var executionResult = await cliExecutor.Execute(deployOptions);
        if (executionResult.ExitCode != 0) executionResult.OutputDebugInformation();
        Assert.Equal(expectedExitCode, executionResult.ExitCode);

        return new DeployNupkgResult(executionResult);
    }

    public static async Task<DeployAndCreateProcessResult> DeployAndCreateProcess(this CliExecutor cliExecutor, string sourceProjectFolder, OrchestratorConnection connection, string[]? environments = null)
    {
        if (connection.IsClassicFolder)
            environments ??= new string[] { connection.ClassicFolderProductionEnvironments.First().EnvironmentName };

        var executionResult = await cliExecutor.Deploy(sourceProjectFolder, connection, createProcess: true, environments: environments);
        var suffix = connection.IsClassicFolder ? environments.First() : Entries.DefaultEntryPoint;
        var createdProcessName = $"{executionResult.DeployedProjectName}_{suffix}";

        return new DeployAndCreateProcessResult(executionResult)
        {
            CreatedProcessName = createdProcessName,
        };
    }

    public static async Task<CliExecutionResult> ManageAssets<T>(this CliExecutor cliExecutor, OrchestratorConnection connection, string assetsFile, bool populateWithConfigurationDetails = true, bool populateWithTelemetryData = true,
        TracingLevel? traceLevel = TracingLevel.Information, bool disableTelemetry = false) where T : AssetsOptions, new()
    {
        var options = new T
        {
            AssetsFile = assetsFile
        }.PopulateWithConnectionDetails(connection);
        if (populateWithConfigurationDetails)
            options.PopulateWithConfigurationDetails(traceLevel: traceLevel, disableTelemetry: disableTelemetry);
        if (populateWithTelemetryData)
            options.PopulateWithTelemetryData();

        var assetsOptions = new RunOptions
        {
            Type = typeof(T).Name,
            Options = options,
        };

        var executionResult = await cliExecutor.Execute(assetsOptions);
        if (executionResult.ExitCode != 0) executionResult.OutputDebugInformation();
        Assert.Equal(CliExitCode.Success, executionResult.ExitCode);

        return executionResult;
    }

    public static async Task<PackResult> Pack(this CliExecutor cliExecutor, string projectJsonPath, OrchestratorConnection? connection = null, OutputType? outputType = null, bool splitOutput = false,
        string? repositoryUrl = null, string? repositoryCommit = null, string? repositoryBranch = null, string? repositoryType = null, string? projectUrl = null, string? releaseNotes = null,
        bool populateWithConfigurationDetails = true, bool populateWithTelemetryData = true, bool autoVersion = true, string version = null, CliExitCode expectedExitCode = CliExitCode.Success,
        TracingLevel? traceLevel = TracingLevel.Information, bool disableTelemetry = false, bool disableBuiltInNugetFeeds = false)
    {
        var projectJsonPathClone = Common.Utils.GetDirectoryOfPath(projectJsonPath).CloneProjectIntoTempLocation();
        var destinationFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        var options = new PackOptions
        {
            ProjectPath = projectJsonPathClone,
            OutputType = outputType,
            SplitOutput = splitOutput,
            AutoVersion = autoVersion,
            Version = version,
            DestinationFolder = destinationFolder,
            RepositoryUrl = repositoryUrl!,
            RepositoryCommit = repositoryCommit!,
            RepositoryBranch = repositoryBranch!,
            RepositoryType = repositoryType!,
            ProjectUrl = projectUrl!,
            ReleaseNotes = releaseNotes,
            DisableBuiltInNugetFeeds = disableBuiltInNugetFeeds,
        }.PopulateWithConnectionDetails(connection);
        if (populateWithConfigurationDetails)
            options.PopulateWithConfigurationDetails(traceLevel: traceLevel, disableTelemetry: disableTelemetry);
        if (populateWithTelemetryData)
            options.PopulateWithTelemetryData();

        var packOptions = new RunOptions
        {
            Type = typeof(PackOptions).Name,
            Options = options,
        };

        var executionResult = await cliExecutor.Execute(packOptions);
        if (executionResult.ExitCode != expectedExitCode) executionResult.OutputDebugInformation();
        Assert.Equal(expectedExitCode, executionResult.ExitCode);

        return new PackResult(executionResult)
        {
            OutputFolder = destinationFolder,
        };
    }

    public static async Task<CliExecutionResult> RunJob(this CliExecutor cliExecutor, string processName, OrchestratorConnection connection, string parametersFilePath = null, string machine = null, string user = null,
        IEnumerable<string> robots = null, bool populateWithConfigurationDetails = true, bool populateWithTelemetryData = true, RobotType? jobType = null, long? jobsCount = null, JobPriority? priority = null,
        bool failWhenJobFails = true, bool waitForJobCompletion = true, long? timeout = null, string resultFilePath = null, CliExitCode expectedExitCode = CliExitCode.Success, TracingLevel? traceLevel = TracingLevel.Information,
        bool disableTelemetry = false)
    {
        var options = new RunJobOptions
        {
            ProcessName = processName,
            ParametersFilePath = parametersFilePath,

            Machine = machine,
            User = user,
            Robots = robots,

            JobType = jobType,
            JobsCount = jobsCount,
            Priority = priority,

            FailWhenJobFails = failWhenJobFails,
            WaitForJobCompletion = waitForJobCompletion,
            Timeout = timeout,

            ResultFilePath = resultFilePath
        }.PopulateWithConnectionDetails(connection);
        if (populateWithConfigurationDetails)
            options.PopulateWithConfigurationDetails(traceLevel: traceLevel, disableTelemetry: disableTelemetry);
        if (populateWithTelemetryData)
            options.PopulateWithTelemetryData();

        var runJobOptions = new RunOptions
        {
            Type = typeof(RunJobOptions).Name,
            Options = options,
        };

        var executionResult = await cliExecutor.Execute(runJobOptions);
        if (executionResult.ExitCode != expectedExitCode) executionResult.OutputDebugInformation();
        Assert.Equal(expectedExitCode, executionResult.ExitCode);

        return executionResult;
    }

    public static async Task<RunTestsResult> RunTests(this CliExecutor cliExecutor, OrchestratorConnection connection, string sourceProjectFolder = null, string testSet = null, string environment = null,
        bool populateWithConfigurationDetails = true, bool populateWithTelemetryData = true, TestReportType? testReportType = null, string testReportDestination = null, long? timeout = null,
        string parametersFilePath = null, bool attachRobotLogs = false, 
        string? repositoryUrl = null, string? repositoryCommit = null, string? repositoryBranch = null, string? repositoryType = null, string? projectUrl = null, string? releaseNotes = null,
        CliExitCode expectedExitCode = CliExitCode.Success, TracingLevel? traceLevel = TracingLevel.Information, bool disableTelemetry = false, bool disableBuiltInNugetFeeds = false, int? retryCount = null)
    {
        var projectJsonPath = sourceProjectFolder.CloneProjectIntoTempLocation();
        var newUniqueProjectName = EnsureUniqueProjectName(projectJsonPath);
        EnsureUniqueTestCaseIds(projectJsonPath);

        var options = new RunTestsOptions
        {
            ProjectPath = projectJsonPath,
            TestSet = testSet,

            Environment = environment,
            TestReportType = testReportType,
            TestReportDestination = testReportDestination,

            ParametersFilePath = parametersFilePath,
            AttachRobotLogs = attachRobotLogs,

            RepositoryUrl = repositoryUrl!,
            RepositoryCommit = repositoryCommit!,
            RepositoryBranch = repositoryBranch!,
            RepositoryType = repositoryType!,
            ProjectUrl = projectUrl!,
            ReleaseNotes = releaseNotes,

            DisableBuiltInNugetFeeds = disableBuiltInNugetFeeds,
            RetryCount = retryCount,
            
            Timeout = timeout,
        }.PopulateWithConnectionDetails(connection);
        if (populateWithConfigurationDetails)
            options.PopulateWithConfigurationDetails(traceLevel: traceLevel, disableTelemetry: disableTelemetry);
        if (populateWithTelemetryData)
            options.PopulateWithTelemetryData();

        var runTestsOptions = new RunOptions
        {
            Type = typeof(RunTestsOptions).Name,
            Options = options,
        };

        var executionResult = await cliExecutor.Execute(runTestsOptions);
        if (executionResult.ExitCode != expectedExitCode) executionResult.OutputDebugInformation();
        Assert.Equal(expectedExitCode, executionResult.ExitCode);

        return new RunTestsResult(executionResult)
        {
            DeployedProjectName = newUniqueProjectName,
        };
    }

    private static string? CloneProjectIntoTempLocation(this string? sourceProjectPath)
    {
        if (sourceProjectPath is null) return null;

        var projectFolderCopy = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(projectFolderCopy);

        foreach (var sourceFile in Directory.GetFiles(sourceProjectPath))
        {
            var extension = Path.GetExtension(sourceFile);
            if (extension == ".json" || extension == ".xaml")
            {
                var fileCopyDestination = sourceFile.Replace(sourceProjectPath, projectFolderCopy);
                File.Copy(sourceFile, fileCopyDestination);
            }
        }

        return Path.Combine(projectFolderCopy, Entries.ProjectJson);
    }

    private static string? EnsureUniqueProjectName(string? projectJsonPath, string? uniqueName = null)
    {
        if (projectJsonPath is null) return null;

        var projectJsonString = File.ReadAllText(projectJsonPath);
        var projectJsonObject = JObject.Parse(projectJsonString);

        uniqueName ??= $"{projectJsonObject["name"]}_{Guid.NewGuid()}";
        projectJsonObject["name"] = uniqueName;
        File.WriteAllText(projectJsonPath, projectJsonObject.ToString());

        return uniqueName;
    }

    private static void EnsureUniqueTestCaseIds(string? projectJsonPath)
    {
        if (projectJsonPath is null) return;

        var projectJsonString = File.ReadAllText(projectJsonPath);
        var projectJsonObject = JObject.Parse(projectJsonString);

        var testCaseObjects = projectJsonObject["designOptions"]["fileInfoCollection"];
        foreach (var testCaseObject in testCaseObjects)
        {
            testCaseObject["testCaseId"] = Guid.NewGuid().ToString();
        }

        File.WriteAllText(projectJsonPath, projectJsonObject.ToString());
    }

    private static void OutputDebugInformation(this CliExecutionResult result)
    {
        result.ErrorLines.ForEach(line => Console.WriteLine(line));
        result.OutputLines.ForEach(line => Console.WriteLine(line));
    }
}
