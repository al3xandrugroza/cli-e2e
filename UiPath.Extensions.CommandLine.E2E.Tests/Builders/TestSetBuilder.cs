using UiPath.Extensions.CommandLine.E2E.Tests.Dtos.InputParam;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Builders;

internal class TestSetBuilder
{
    private readonly TestSetDto _testSetDto;

    public TestSetBuilder()
    {
        _testSetDto = new TestSetDto
        {
            Description = "Created during RunExistingTestSuite e2e test.",
            EnableCoverage = false,
        };
    }

    public static TestSetBuilder Init()
    {
        return new TestSetBuilder();
    }

    public TestSetBuilder WithName(string name)
    {
        _testSetDto.Name = name;
        return this;
    }

    public TestSetBuilder WithPackageIdentifier(string packageIdentifier, string testSetPackageVersion, int testCaseDefinitionId, int testCaseReleaseId)
    {
        _testSetDto.Packages = new[]
        {
            new TestSetPackageDto {
                PackageIdentifier = packageIdentifier,
                VersionMask = "1.0.*",
                IncludePrerelease = false
            }
        };
        _testSetDto.TestCases = new[]
        {
            new TestCaseDto
            {
                DefinitionId = testCaseDefinitionId,
                Enabled = true,
                ReleaseId = testCaseReleaseId,
                VersionNumber = testSetPackageVersion
            }
        };

        return this;
    }

    public TestSetBuilder WithEnvironment(int environmentId)
    {
        _testSetDto.EnvironmentId = environmentId;
        return this;
    }

    public TestSetBuilder WithInputArgs()
    {
        _testSetDto.InputArguments = new[]
        {
            new TestSetInputArgumentDto
            {
                Id = 0,
                Name = "flag",
                Type = ParamType.Bool,
                Value = "false"
            }
        };

        return this;
    }

    public TestSetDto Build()
    {
        return _testSetDto;
    }
}
