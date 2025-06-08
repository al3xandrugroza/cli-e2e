using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Xml;
using Newtonsoft.Json.Linq;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options.Enums;
using UiPath.Extensions.CommandLine.E2E.Tests.Dtos;
using System.Text;
using NuGet.Packaging;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Common;

internal static class Utils
{
    // Test change for PR

    public static void AssertTestReport(string reportPath, string exepectedProjectName, int expectedNumberOfTests = 1, int expectedNumberOfFailures = 0,
        int expectedNumberOfErrors = 0, int expectedNumberOfCancellations = 0, TestReportType reportType = TestReportType.uipath, bool robotLogsAttachment = false)
    {
        switch (reportType)
        {
            case TestReportType.junit:
                AssertJunitTestReport(reportPath, exepectedProjectName, expectedNumberOfTests, expectedNumberOfFailures, expectedNumberOfErrors, expectedNumberOfCancellations, robotLogsAttachment);
                break;
            case TestReportType.uipath:
                AssertUipathTestReport(reportPath, exepectedProjectName, expectedNumberOfTests, expectedNumberOfFailures, expectedNumberOfErrors, expectedNumberOfCancellations, robotLogsAttachment);
                break;
        }
    }

        public static void AssertJobResults(string resultsPath, int expectedNumberOfJobExecutions = 1, string expectedStatus = "Successful")
    {
        var resultsJsonString = File.ReadAllText(resultsPath);
        var resultsJson = JObject.Parse(resultsJsonString);

        Assert.Equal(expectedStatus, resultsJson["status"].ToString());
        Assert.Equal(expectedNumberOfJobExecutions, resultsJson["result"].Count());
    }

    public static ISet<AnalyzeMessage> GetAnalyzeMessagesFromFile(string resultFilePath)
    {
        var jsonString = File.ReadAllText(resultFilePath);
        return JsonConvert.DeserializeObject<ISet<AnalyzeMessage>>(jsonString);
    }

    public static string GetRandomXmlFileInTempPath()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        return Path.ChangeExtension(tempFile, ".xml");
    }

    public static string GetRandomJsonFileInTempPath()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        return Path.ChangeExtension(tempFile, ".json");
    }

    public static string GetRandomCsvFileInTempPath()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        return Path.ChangeExtension(tempFile, ".csv");
    }

    public static async Task ExecuteIgnoreExceptionAsync<T>(Func<Task> task = null, Action action = null) where T : Exception
    {
        try
        {
            if (task is not null)
                await task();
            if (action is not null)
                action();
        }
        catch (T)
        {
            // ignore
        }
    }

    public static void WriteObjectAsJson(string path, object obj)
    {
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };
        string inputParamsJsonString = JsonConvert.SerializeObject(obj, settings);
        File.WriteAllText(path, inputParamsJsonString);
    }

    public static string ExtractVersionFromProjectJson(string projectJsonPath)
    {
        var projectJsonString = File.ReadAllText(projectJsonPath);
        var projectJsonObject = JObject.Parse(projectJsonString);

        return projectJsonObject["projectVersion"].ToString();
    }

    public static void DeleteCachedPackage(string packageName)
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var cachedPackagePath = Path.Combine(userProfile, ".nuget", "packages", packageName);

        if (Directory.Exists(cachedPackagePath))
            Directory.Delete(cachedPackagePath, true);
    }

    public static string CreateDeployAssetsFile(Dictionary<string, (string AssetType, object AssetValue)> assets)
    {
        var content = new StringBuilder();
        content.AppendLine("name,type,value");

        foreach (var asset in assets)
        {
            content.AppendLine($"{asset.Key},{asset.Value.AssetType},{asset.Value.AssetValue}");
        }

        var deployAssetsPath = Utils.GetRandomCsvFileInTempPath();
        File.WriteAllText(deployAssetsPath, content.ToString());

        return deployAssetsPath;
    }

    public static string GetDirectoryOfPath(string path)
    {
        return Directory.Exists(path) ? path : Path.GetDirectoryName(path);
    }
    
    public static void ReadAndAssertPackageMetadata(PackageArchiveReader packageReader, PackageMetadata expectedPackageMetadata)
    {
        var nuspecReader = packageReader.NuspecReader;
        var actualPackageMetadata = nuspecReader.GetRepositoryMetadata();
        var actualProjectUrl = nuspecReader.GetMetadataValue("projectUrl");
        var actualReleaseNotes = nuspecReader.GetMetadataValue("releaseNotes");

        Assert.Equal(expectedPackageMetadata.RepositoryUrl, actualPackageMetadata.Url);
        Assert.Equal(expectedPackageMetadata.RepositoryBranch, actualPackageMetadata.Branch);
        Assert.Equal(expectedPackageMetadata.RepositoryCommit, actualPackageMetadata.Commit);
        Assert.Equal(expectedPackageMetadata.RepositoryType, actualPackageMetadata.Type);
        Assert.Equal(expectedPackageMetadata.ProjectUrl, actualProjectUrl);
        Assert.Equal(expectedPackageMetadata.ReleaseNotes, actualReleaseNotes);
    }

    private static void AssertUipathTestReport(string reportPath, string expectedProjectName, int expectedNumberOfTests, int expectedNumberOfFailures, int expectedNumberOfErrors, int expectedNumberOfCancellations, bool robotLogsAttachment)
    {
        var reportJsonString = File.ReadAllText(reportPath);
        var reportJsonObject = JObject.Parse(reportJsonString);

        var executionObject = reportJsonObject["TestSetExecutions"].First;
        var reportedTestSuiteName = executionObject["Name"].ToString();
        var reportedNumberOfErrors = int.Parse(executionObject["ErrorsCount"].ToString());
        var reportedNumberOfCancellations = int.Parse(executionObject["CanceledCount"].ToString());
        var reportedNumberOfTests = int.Parse(executionObject["TestCasesCount"].ToString());
        var reportedNumberOfFailures = int.Parse(executionObject["FailuresCount"].ToString());
        var reportedRobotLogs = executionObject["TestCaseExecutions"].First["RobotLogs"];

        Assert.Contains(expectedProjectName, reportedTestSuiteName);
        Assert.Equal(expectedNumberOfTests, reportedNumberOfTests);
        Assert.Equal(expectedNumberOfFailures, reportedNumberOfFailures);
        Assert.Equal(expectedNumberOfErrors, reportedNumberOfErrors);
        Assert.Equal(expectedNumberOfCancellations, reportedNumberOfCancellations);
        Assert.Equal(robotLogsAttachment, reportedRobotLogs.HasValues);
    }

    private static void AssertJunitTestReport(string reportPath, string exepectedProjectName, int expectedNumberOfTests, int expectedNumberOfFailures, int expectedNumberOfErrors, int expectedNumberOfCancellations, bool robotLogsAttachment)
    {
        var report = new XmlDocument();
        report.Load(reportPath);
        var reportedTestSuite = report.DocumentElement.FirstChild.Attributes;
        var reportedNumberOfTests = int.Parse(reportedTestSuite["tests"].Value);
        var reportedNumberOfFailures = int.Parse(reportedTestSuite["failures"].Value);
        var reportedNumberOfErrors = int.Parse(reportedTestSuite["errors"].Value);
        var reportedNumberOfCancellations = int.Parse(reportedTestSuite["cancelled"].Value);
        var reportedTestSuiteName = reportedTestSuite["name"].Value;
        var reportedSystemOutput = report.DocumentElement.FirstChild.FirstChild.FirstChild.InnerText;

        Assert.Contains(exepectedProjectName, reportedTestSuiteName);
        Assert.Equal(expectedNumberOfTests, reportedNumberOfTests);
        Assert.Equal(expectedNumberOfFailures, reportedNumberOfFailures);
        Assert.Equal(expectedNumberOfErrors, reportedNumberOfErrors);
        Assert.Equal(expectedNumberOfCancellations, reportedNumberOfCancellations);
        Assert.Equal(robotLogsAttachment, reportedSystemOutput.Contains("Robot logs [{"));
    }
}
