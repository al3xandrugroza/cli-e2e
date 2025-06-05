namespace UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;

public class OrchestratorConnection
{
    public string ConnectionName { get; set; }

    public string BaseUrl { get; set; }
    public string AuthorizationUrl { get; set; }
    public string OrchestratorUrl { get; set; }

    public string AccountName { get; set; }
    public string AccountForApp { get; set; }
    public string OrchestratorTenant { get; set; }
    public string OrganizationUnit { get; set; }
    public bool IsClassicFolder {  get; set; }
    public string OrganizationUnitId { get; set; }
    public Guid? FolderFeedId { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }

    public string RefreshToken { get; set; }

    public string ApplicationId { get; set; }
    public string ApplicationSecret { get; set; }
    public string ApplicationScope { get; set; }

    public Func<HttpClient> GetClientWithIdentityHandler { get; set; }

    public IEnumerable<ClassicFolderEnvironment> ClassicFolderProductionEnvironments { get; } = new ClassicFolderEnvironment[]
    {
        new ClassicFolderEnvironment
        {
            EnvironmentName = "CIPlugins-Production-Environment-1",
            RobotName = "cls_robo_std_prd_1",
        },
        new ClassicFolderEnvironment
        {
            EnvironmentName = "CIPlugins-Production-Environment-2",
            RobotName = "cls_robo_std_prd_2",
        },
    };

    public IEnumerable<ClassicFolderEnvironment> ClassicFolderTestingEnvironments { get; } = new ClassicFolderEnvironment[]
    {
        new ClassicFolderEnvironment
        {
            EnvironmentName = "CIPlugins-Testing-Environment-3",
            RobotName = "cls_robo_std_tst_3"
        },
    };

    public IEnumerable<RobotHost> ModernFolderRobotHosts { get; set; }

    public int SuccessfulTestSetEnvironmentId { get; set; }
    public string SuccessfulTestSetPackageVersion { get; set; }
    public int SuccessfulTestCaseDefinitionId { get; set; }
    public int SuccessfulTestCaseReleaseId { get; set; }
    
    public int FailingTestSetEnvironmentId { get; set; }
    public string FailingTestSetPackageVersion { get; set; }
    public int FailingTestCaseDefinitionId { get; set; }
    public int FailingTestCaseReleaseId { get; set; }
}
