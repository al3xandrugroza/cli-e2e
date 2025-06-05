namespace UiPath.Extensions.CommandLine.E2E.Tests.Dtos;

public class PackageMetadata
{
    public string RepositoryUrl { get; init; }
    public string RepositoryCommit { get; init; }
    public string RepositoryBranch { get; init; }
    public string RepositoryType { get; init; }
    public string ProjectUrl { get; init; }
    public string ReleaseNotes { get; init; }
}