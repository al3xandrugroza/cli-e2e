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
    
    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task AnalyzeWithGovernanceFile(CliExecutor cliExecutor)
    {
        const string analyzeSuccessLog = "Done analyzing project";
        const string analyzeErrorLog = "Done analyzing with errors.";

        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.Windows, Entries.VB, Entries.SimpleLog_Windows_VB);
        var policyWithoutErrorRules = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.GovernanceFiles, Entries.GovernanceWithWarningTraceLevel);
        var policyWithErrorRules = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.GovernanceFiles, Entries.GovernanceWithErrorTraceLevel);

        var result = await cliExecutor.Analyze(projectJsonPath, governanceFilePath: policyWithoutErrorRules);
        Assert.Contains(result.OutputLines, line => line.Contains(analyzeSuccessLog));
        Assert.DoesNotContain(result.OutputLines, line => line.Contains(analyzeErrorLog));
        
        result = await cliExecutor.Analyze(projectJsonPath, governanceFilePath: policyWithErrorRules, expectedExitCode: CliExitCode.Failure);
        Assert.Contains(result.OutputLines, line => line.Contains(analyzeErrorLog));
    }

    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task OutputResultFile(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.Windows, Entries.VB, Entries.SimpleLog_Windows_VB);
        var resultFilePath = Common.Utils.GetRandomJsonFileInTempPath();
        var policyWithErrorRules = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.GovernanceFiles, Entries.GovernanceWithErrorTraceLevel);

        await cliExecutor.Analyze(projectJsonPath, governanceFilePath: policyWithErrorRules, expectedExitCode: CliExitCode.Failure, resultFilePath: resultFilePath);
        var expectedAnalyzeMessages = new HashSet<AnalyzeMessage> {
            new AnalyzeMessage
            {
                ErrorCode = "ST-USG-034",
                ErrorSeverity = TraceLevel.Error
            }
        };
        var analyzeMessages = Common.Utils.GetAnalyzeMessagesFromFile(resultFilePath);
        Assert.Subset(analyzeMessages, expectedAnalyzeMessages);
    }

    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task StopOnRuleViolation(CliExecutor cliExecutor)
    {
        var resultFilePath = Common.Utils.GetRandomJsonFileInTempPath();
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.Windows, Entries.VB, Entries.SimpleLog_Windows_VB);
        var policyWithErrorRules = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.GovernanceFiles, Entries.GovernanceWithErrorTraceLevel);

        await cliExecutor.Analyze(projectJsonPath, governanceFilePath: policyWithErrorRules, resultFilePath: resultFilePath, expectedExitCode: CliExitCode.Failure);
        await cliExecutor.Analyze(projectJsonPath, resultFilePath: resultFilePath, stopOnRuleViolation: false);
    }

    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task AcceptProjectJsonFileOrDirectory(CliExecutor cliExecutor)
    {
        var policyWithoutErrorRules = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.GovernanceFiles, Entries.GovernanceWithWarningTraceLevel);
        var projectJsonDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.Windows, Entries.VB, Entries.SimpleLog_Windows_VB);
        var projectJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.Windows, Entries.VB, Entries.SimpleLog_Windows_VB, Entries.ProjectJson);
        var resultFilePath = Common.Utils.GetRandomJsonFileInTempPath();

        await cliExecutor.Analyze(projectJsonDirectory, governanceFilePath: policyWithoutErrorRules, resultFilePath: resultFilePath);

        resultFilePath = Common.Utils.GetRandomJsonFileInTempPath();
        await cliExecutor.Analyze(projectJsonFilePath, governanceFilePath: policyWithoutErrorRules, resultFilePath: resultFilePath);
    }

    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task DisableHardcodedFeeds(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.ProjectWithCustomLibraryFromOrchestrator_CrossPlatform_VB);
        var cachedPackages = new string[] { "library_crossplatform_vb", "library_crossplatform_vb.library.runtime" };
        var builtInFeed = "https://pkgs.dev.azure.com/uipath/Public.Feeds/_packaging/UiPath-Official/nuget/v3/index.json";

        foreach (var cachedPackage in cachedPackages)
        {
            Common.Utils.DeleteCachedPackage(cachedPackage);
        }

        var result = await cliExecutor.Analyze(projectJsonPath, expectedExitCode: CliExitCode.Failure, analyzerTraceLevel: TraceLevel.Warning);
        Assert.Contains(result.OutputLines, line => line.Contains(builtInFeed));
        result = await cliExecutor.Analyze(projectJsonPath, expectedExitCode: CliExitCode.Failure, analyzerTraceLevel: TraceLevel.Warning, disableBuiltInNugetFeeds: true);
        Assert.DoesNotContain(result.OutputLines, line => line.Contains(builtInFeed));
    }
}
