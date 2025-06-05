using UiPath.Extensions.CommandLine.E2E.Tests.Dtos;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Builders;

internal class PackageMetadataBuilder
{
    private PackageMetadata _packageMetadata = new();

    public static PackageMetadataBuilder Init()
    {
        return new PackageMetadataBuilder();
    }

    public PackageMetadataBuilder APackageMetadata()
    {
        _packageMetadata = new PackageMetadata
        {
            RepositoryUrl = "https://github.com/TestGitHubOrganization/TestRepositoryName/blob/branchName/RepoDirectory/TestAutomationProject/project.json",
            RepositoryCommit = "811a85f631e03e33bbcc3701f0abbf96a23fe498",
            RepositoryBranch = "branchName",
            RepositoryType = "git",
            ProjectUrl = "https://alpha.uipath.com/TestOrganization/TestTenant/automationhub_/automation-profile/Test-Idea",
            ReleaseNotes = "Random release notes: " + Guid.NewGuid().ToString()
        };
        return this;
    }

    public PackageMetadata Build()
    {
        return _packageMetadata;
    }
}