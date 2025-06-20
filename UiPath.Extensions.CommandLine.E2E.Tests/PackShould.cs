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
        // some changes
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);
        
        var result = await cliExecutor.Pack(projectJsonPath, outputType: OutputType.Process);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB);
    }

    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [WindowsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task PackWithTheOutputTypeSpecifiedInProjectJsonByDefault(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);
        
        var result = await cliExecutor.Pack(projectJsonPath);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB);
    }

    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task PassTheOutputTypeToPackager(CliExecutor cliExecutor)
    {
        var processProjectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);
        var libraryProjectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.Library_CrossPlatform_VB, Entries.ProjectJson);
        
        await cliExecutor.Pack(processProjectJsonPath, outputType: OutputType.Library, expectedExitCode: CliExitCode.Failure);
        await cliExecutor.Pack(libraryProjectJsonPath, outputType: OutputType.Process, expectedExitCode: CliExitCode.Failure);

        var result = await cliExecutor.Pack(processProjectJsonPath, outputType: OutputType.Process);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB);

        result = await cliExecutor.Pack(libraryProjectJsonPath, outputType: OutputType.Library);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.Library_CrossPlatform_VB);
    }

    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task OutputAPackageOnWindowsGivenAWindowsProcess(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.Windows, Entries.VB, Entries.SimpleLog_Windows_VB, Entries.ProjectJson);
        
        var result = await cliExecutor.Pack(projectJsonPath);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_Windows_VB);
    }

    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task PackWithCustomVersion(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);
        var currentVersion = Common.Utils.ExtractVersionFromProjectJson(projectJsonPath);
        var customVersion = currentVersion + "." + DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        var result = await cliExecutor.Pack(projectJsonPath, autoVersion: false, version: customVersion);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB, version: customVersion);
    }

    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task AcceptProjectJsonFileOrDirectory(CliExecutor cliExecutor)
    {
        var projectJsonDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB);
        var projectJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);

        var result = await cliExecutor.Pack(projectJsonDirectory);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB);
        
        result = await cliExecutor.Pack(projectJsonFilePath);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB);
    }

    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [WindowsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task PackWithVersionSpecifiedInProjectJson(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);
        var currentVersion = Common.Utils.ExtractVersionFromProjectJson(projectJsonPath);

        var result = await cliExecutor.Pack(projectJsonPath, autoVersion: false);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB, version: currentVersion);
    }

    [Theory]
    [CrossPlatformCli]
    [CrossPlatformCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task PackWithAutoVersion(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);
        var currentVersion = Common.Utils.ExtractVersionFromProjectJson(projectJsonPath);

        var result = await cliExecutor.Pack(projectJsonPath);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB, autoVersion: true, version: currentVersion);
    }
    
    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task PackAndSplitLibrary(CliExecutor cliExecutor)
    {
        const bool splitOutput = true;
        var libraryProjectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.CS, Entries.LibraryForSplit_CrossPlatform_VB, Entries.ProjectJson);

        var result = await cliExecutor.Pack(libraryProjectJsonPath, outputType: OutputType.Library);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.LibraryForSplit_CrossPlatform_VB, outputType: OutputType.Library);
        
        result = await cliExecutor.Pack(libraryProjectJsonPath, outputType: OutputType.Library, splitOutput: splitOutput);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.LibraryForSplit_CrossPlatform_VB, splitOutput: splitOutput, outputType: OutputType.Library);
    }
    
    [Theory]
    [WindowsCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task PackProcessAndSeparateRuntimeDependencies(CliExecutor cliExecutor)
    {
        const bool splitOutput = true;
        var libraryProjectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.ProjectForSeparateRuntimeDependencies_CrossPlatform_VB, Entries.ProjectJson);

        var result = await cliExecutor.Pack(libraryProjectJsonPath, outputType: OutputType.Process);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.ProjectForSeparateRuntimeDependencies_CrossPlatform_VB, outputType: OutputType.Process);
        
        result = await cliExecutor.Pack(libraryProjectJsonPath, outputType: OutputType.Process, splitOutput: splitOutput);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.ProjectForSeparateRuntimeDependencies_CrossPlatform_VB, splitOutput: splitOutput, outputType: OutputType.Process);
    }
    
    [Theory]
    [CrossPlatformCli]
    [WindowsCliWithInlineArgs]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    [Trait(TraitConsts.Agent, TraitConsts.Windows)]
    public async Task PackWithRepositoryMetadata(CliExecutor cliExecutor)
    {
        var projectJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Entries.AutomationProjects, Entries.CrossPlatform, Entries.VB, Entries.SimpleLog_CrossPlatform_VB, Entries.ProjectJson);

        var packageMetadata = PackageMetadataBuilder.Init().APackageMetadata().Build();
        
        var result = await cliExecutor.Pack(projectJsonPath, outputType: OutputType.Process, repositoryUrl: packageMetadata.RepositoryUrl, repositoryCommit: packageMetadata.RepositoryCommit, 
                                            repositoryBranch: packageMetadata.RepositoryBranch, repositoryType: packageMetadata.RepositoryType, projectUrl: packageMetadata.ProjectUrl, releaseNotes: packageMetadata.ReleaseNotes);
        AssertDestinationFolderContainsNupkgWithGivenMetadata(result.OutputFolder, Entries.SimpleLog_CrossPlatform_VB, packageMetadata: packageMetadata);
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
        var result = await cliExecutor.Pack(projectJsonPath, expectedExitCode: CliExitCode.Failure);
        Assert.Contains(result.OutputLines, line => line.Contains(builtInFeed));
        result = await cliExecutor.Pack(projectJsonPath, expectedExitCode: CliExitCode.Failure, disableBuiltInNugetFeeds: true);
        Assert.DoesNotContain(result.OutputLines, line => line.Contains(builtInFeed));
    }

    private static void AssertDestinationFolderContainsNupkgWithGivenMetadata(string folder, string packageName, PackageMetadata? packageMetadata = null, 
                bool autoVersion = false, string? version = null, bool splitOutput = false, OutputType? outputType = null)
    {
        var files = Directory.GetFiles(folder);
        var nugetFilePath = files.First(file => !file.Contains(".Runtime."));
        var expectedOutputFilesCount = 1;
        
        if (splitOutput && outputType == OutputType.Library) {
            var runtimePackageFilePath = files.First(file => file.Contains(".Runtime."));
            var runtimePackageName = $"{packageName}.Library.Runtime";
            expectedOutputFilesCount = 2;
            AssertNuPkgEqualsPackageMetadata(runtimePackageName, runtimePackageFilePath);
        }
        
        var packageIdentity = AssertNuPkgEqualsPackageMetadata(packageName, nugetFilePath, packageMetadata, splitOutput: splitOutput, outputType: outputType);
        Assert.Equal(expectedOutputFilesCount, files.Length);

        if (version is not null)
            if (autoVersion)
                Assert.DoesNotMatch(version, packageIdentity.Version.ToString());
            else
                Assert.Equal(version, packageIdentity.Version.ToString());
    }
    
    private static PackageIdentity AssertNuPkgEqualsPackageMetadata(string packageName, string nugetFilePath, PackageMetadata? packageMetadata = null, bool splitOutput = false, OutputType? outputType = null)
    {
        if (string.IsNullOrEmpty(packageName) || string.IsNullOrEmpty(nugetFilePath)) {
            throw new ArgumentNullException($"Either packageName: {packageName} or nugetFilePath: {nugetFilePath} were null or empty.");
        }

        var packageReader = new PackageArchiveReader(nugetFilePath);
        var packageIdentity = packageReader.GetIdentity();

        Assert.Contains($"{packageIdentity.Id}.{packageIdentity.Version}.nupkg", nugetFilePath);
        Assert.Equal(packageName, packageIdentity.Id);
        
        if (packageMetadata is not null) {
            Common.Utils.ReadAndAssertPackageMetadata(packageReader, packageMetadata);
        }
        
        if (outputType == OutputType.Process) {
            if (splitOutput) {
                Assert.Contains(packageReader.NuspecReader.GetDependencyGroups().First().Packages, package => package.Id.Contains(".Runtime"));
            } else {
                Assert.DoesNotContain(packageReader.NuspecReader.GetDependencyGroups().First().Packages, package => package.Id.Contains(".Runtime"));
            }
        }
        
        return packageIdentity;
    }
}