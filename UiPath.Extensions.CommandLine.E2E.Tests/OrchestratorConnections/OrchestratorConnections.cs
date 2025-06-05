using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Utils;
using UiPath.Extensions.CommandLine.E2E.Tests.IdentityHandlers.Handlers;

namespace UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;

internal static class OrchestratorConnections
{
    public static OrchestratorConnection UserPassClassicFolderPaas_21_10 { get; } = new OrchestratorConnection
    {
        ConnectionName = nameof(UserPassClassicFolderPaas_21_10),

        BaseUrl = "https://orch-21-10.westeurope.cloudapp.azure.com/",
        OrchestratorUrl = "https://orch-21-10.westeurope.cloudapp.azure.com/",

        Username = "admin",
        Password = Environment.GetEnvironmentVariable(AgentEnvironment.Orchestrator_21_10_Password),
        OrchestratorTenant = "ciplugins",

        OrganizationUnit = "CIPlugins-ClassicFolder",
        IsClassicFolder = true,
        OrganizationUnitId = "17",
        
        GetClientWithIdentityHandler = () => IdentityHandlers.Handlers.IdentityHandlers
            .NewIdentityConfiguration(UserPassClassicFolderPaas_21_10.BaseUrl.AsIdentityServiceUrl(isMsiDeployment: true))
            .WithUsernamePassword(
                UserPassClassicFolderPaas_21_10.OrchestratorUrl,
                UserPassClassicFolderPaas_21_10.Username,
                UserPassClassicFolderPaas_21_10.Password,
                UserPassClassicFolderPaas_21_10.OrchestratorTenant)
            .UsernamePasswordTokenHandler()
            .CreateHttpClient(baseAddress: UserPassClassicFolderPaas_21_10.OrchestratorUrl)
            .AddOrganizationUnitHeader(UserPassClassicFolderPaas_21_10.OrganizationUnitId),

        SuccessfulTestSetEnvironmentId = 12,
        SuccessfulTestSetPackageVersion = "1.0.171235156",
        SuccessfulTestCaseDefinitionId = 14141,
        SuccessfulTestCaseReleaseId = 187463,
        
        FailingTestSetEnvironmentId = 12,
        FailingTestSetPackageVersion = "1.0.171255661",
        FailingTestCaseDefinitionId = 14150,
        FailingTestCaseReleaseId = 187494,
    };

    public static OrchestratorConnection UserPassModernFolderPaas_23_4 { get; } = new OrchestratorConnection
    {
        ConnectionName = nameof(UserPassModernFolderPaas_23_4),

        BaseUrl = "https://orch-23-4.westeurope.cloudapp.azure.com/",
        OrchestratorUrl = "https://orch-23-4.westeurope.cloudapp.azure.com/",

        Username = "admin",
        Password = Environment.GetEnvironmentVariable(AgentEnvironment.Orchestrator_23_4_Password),
        OrchestratorTenant = "ciplugins",

        OrganizationUnit = "CIPlugins",
        OrganizationUnitId = "15",
        FolderFeedId = new Guid("ea048e10-567b-42a9-8906-0204dab99731"),

        GetClientWithIdentityHandler = () => IdentityHandlers.Handlers.IdentityHandlers
                                                             .NewIdentityConfiguration(UserPassModernFolderPaas_23_4.BaseUrl.AsIdentityServiceUrl(isMsiDeployment: true))
                                                             .WithUsernamePassword(
                                                                 UserPassModernFolderPaas_23_4.OrchestratorUrl,
                                                                 UserPassModernFolderPaas_23_4.Username,
                                                                 UserPassModernFolderPaas_23_4.Password,
                                                                 UserPassModernFolderPaas_23_4.OrchestratorTenant)
                                                             .UsernamePasswordTokenHandler()
                                                             .CreateHttpClient(baseAddress: UserPassModernFolderPaas_23_4.OrchestratorUrl)
                                                             .AddOrganizationUnitHeader(UserPassModernFolderPaas_23_4.OrganizationUnitId),

        ModernFolderRobotHosts = new RobotHost[]
        {
            new RobotHost
            {
                MachineHostname = "ORCH-23-4-MODER",
                MachineUsername = "orch-23-4-moder\\uipathadmin",
            },
            new RobotHost
            {
                MachineHostname = "ORCH-23-4-2MODE",
                MachineUsername = "orch-23-4-2mode\\uipathadmin",
            },
        },

        SuccessfulTestSetPackageVersion = "1.0.171228354",
        SuccessfulTestCaseDefinitionId = 10368,
        SuccessfulTestCaseReleaseId = 20008,
        
        FailingTestSetPackageVersion = "1.0.171255661",
        FailingTestCaseDefinitionId = 10368,
        FailingTestCaseReleaseId = 90623,
    };

    public static OrchestratorConnection ExtAppModernFolderPaas_21_10 { get; } = new OrchestratorConnection
    {
        ConnectionName = nameof(ExtAppModernFolderPaas_21_10),

        BaseUrl = "https://orch-21-10.westeurope.cloudapp.azure.com/",
        OrchestratorUrl = "https://orch-21-10.westeurope.cloudapp.azure.com/",
        AuthorizationUrl = "https://orch-21-10.westeurope.cloudapp.azure.com/identity",

        AccountForApp = null,
        OrchestratorTenant = "ciplugins",
        OrganizationUnit = "CIPlugins-ModernFolder",
        OrganizationUnitId = "18",
        FolderFeedId = new Guid("ecc8a476-31f3-4f80-a4f2-11114fbf4d25"),

        ApplicationId = "56032552-3e4f-4530-93a7-cdb426f91f1d",
        ApplicationSecret = Environment.GetEnvironmentVariable(AgentEnvironment.Orchestrator_21_10_ExternalAppSecret),
        ApplicationScope = "OR.Folders OR.BackgroundTasks OR.TestSets OR.TestSetExecutions OR.TestSetSchedules OR.Settings.Read OR.Robots.Read OR.Machines.Read OR.Execution OR.Assets OR.Users.Read OR.Jobs OR.Monitoring",

        GetClientWithIdentityHandler = () => IdentityHandlers.Handlers.IdentityHandlers
                                                             .NewIdentityConfiguration(ExtAppModernFolderPaas_21_10.BaseUrl.AsIdentityServiceUrl(isMsiDeployment: true))
                                                             .WithExternalApp(
                                                                 ExtAppModernFolderPaas_21_10.ApplicationId,
                                                                 ExtAppModernFolderPaas_21_10.ApplicationSecret,
                                                                 ExtAppModernFolderPaas_21_10.ApplicationScope)
                                                             .ExternalAppTokenHandler()
                                                             .CreateHttpClient(baseAddress: ExtAppModernFolderPaas_21_10.OrchestratorUrl)
                                                             .AddOrganizationUnitHeader(ExtAppModernFolderPaas_21_10.OrganizationUnitId),

        ModernFolderRobotHosts = new RobotHost[]
        {
            new RobotHost
            {
                MachineHostname = "ORCH-21-10-MODE",
                MachineUsername = "orch-21-10-mode\\uipathadmin",
            },
            new RobotHost
            {
                MachineHostname = "ORCH-21-10-2MOD",
                MachineUsername = "orch-21-10-2mod\\uipathadmin",
            },
        },

        SuccessfulTestSetPackageVersion = "1.0.171228354",
        SuccessfulTestCaseDefinitionId = 10887,
        SuccessfulTestCaseReleaseId = 101018,
        
        FailingTestSetPackageVersion = "1.0.171255661",
        FailingTestCaseDefinitionId = 14151,
        FailingTestCaseReleaseId = 187495,
    };

    public static OrchestratorConnection ExtAppModernFolderPaas_22_4 { get; } = new OrchestratorConnection
    {
        ConnectionName = nameof(ExtAppModernFolderPaas_22_4),

        BaseUrl = "https://orch-22-4.westeurope.cloudapp.azure.com/",
        OrchestratorUrl = "https://orch-22-4.westeurope.cloudapp.azure.com/",
        AuthorizationUrl = "https://orch-22-4.westeurope.cloudapp.azure.com/identity",

        AccountForApp = null,
        OrchestratorTenant = "ciplugins",
        OrganizationUnit = "CIPlugins",
        OrganizationUnitId = "6",
        FolderFeedId = new Guid("7071ca59-4866-4e92-9313-3cf68a429e28"),

        ApplicationId = "d52619ef-b2c9-4950-9b6c-eefc5cdd3520",
        ApplicationSecret = Environment.GetEnvironmentVariable(AgentEnvironment.Orchestrator_22_4_ExternalAppSecret),
        ApplicationScope = "OR.Folders OR.BackgroundTasks OR.TestSets OR.TestSetExecutions OR.TestSetSchedules OR.Settings.Read OR.Robots.Read OR.Machines.Read OR.Execution OR.Assets OR.Users.Read OR.Jobs OR.Monitoring",

        GetClientWithIdentityHandler = () => IdentityHandlers.Handlers.IdentityHandlers
                                                             .NewIdentityConfiguration(ExtAppModernFolderPaas_22_4.BaseUrl.AsIdentityServiceUrl(isMsiDeployment: true))
                                                             .WithExternalApp(
                                                                 ExtAppModernFolderPaas_22_4.ApplicationId,
                                                                 ExtAppModernFolderPaas_22_4.ApplicationSecret,
                                                                 ExtAppModernFolderPaas_22_4.ApplicationScope)
                                                             .ExternalAppTokenHandler()
                                                             .CreateHttpClient(baseAddress: ExtAppModernFolderPaas_22_4.OrchestratorUrl)
                                                             .AddOrganizationUnitHeader(ExtAppModernFolderPaas_22_4.OrganizationUnitId),

        ModernFolderRobotHosts = new RobotHost[]
        {
            new RobotHost
            {
                MachineHostname = "ORCH-22-4-MODER",
                MachineUsername = "orch-22-4-moder\\uipathadmin",
            },
            new RobotHost
            {
                MachineHostname = "ORCH-22-4-2MODE",
                MachineUsername = "orch-22-4-2mode\\uipathadmin",
            },
        },

        SuccessfulTestSetPackageVersion = "1.0.171228354",
        SuccessfulTestCaseDefinitionId = 26,
        SuccessfulTestCaseReleaseId = 10003,
        
        FailingTestSetPackageVersion = "1.0.171255661",
        FailingTestCaseDefinitionId = 290,
        FailingTestCaseReleaseId = 30541,
    };

    public static OrchestratorConnection ExtAppModernFolderPaas_22_10 { get; } = new OrchestratorConnection
    {
        ConnectionName = nameof(ExtAppModernFolderPaas_22_10),

        BaseUrl = "https://orch-22-10.westeurope.cloudapp.azure.com/",
        OrchestratorUrl = "https://orch-22-10.westeurope.cloudapp.azure.com/",
        AuthorizationUrl = "https://orch-22-10.westeurope.cloudapp.azure.com/identity",

        AccountForApp = null,
        OrchestratorTenant = "ciplugins",
        OrganizationUnit = "CIPlugins",
        OrganizationUnitId = "6",
        FolderFeedId = new Guid("e5cdfeda-96a9-4e17-bd4e-9b664f5687a0"),

        ApplicationId = "ab066eca-b8c3-44b1-8188-2dcdde66c9bc",
        ApplicationSecret = Environment.GetEnvironmentVariable(AgentEnvironment.Orchestrator_22_10_ExternalAppSecret),
        ApplicationScope = "OR.Folders OR.BackgroundTasks OR.TestSets OR.TestSetExecutions OR.TestSetSchedules OR.Settings.Read OR.Robots.Read OR.Machines.Read OR.Execution OR.Assets OR.Users.Read OR.Jobs OR.Monitoring",

        GetClientWithIdentityHandler = () => IdentityHandlers.Handlers.IdentityHandlers
                                                             .NewIdentityConfiguration(ExtAppModernFolderPaas_22_10.BaseUrl.AsIdentityServiceUrl(isMsiDeployment: true))
                                                             .WithExternalApp(
                                                                 ExtAppModernFolderPaas_22_10.ApplicationId,
                                                                 ExtAppModernFolderPaas_22_10.ApplicationSecret,
                                                                 ExtAppModernFolderPaas_22_10.ApplicationScope)
                                                             .ExternalAppTokenHandler()
                                                             .CreateHttpClient(baseAddress: ExtAppModernFolderPaas_22_10.OrchestratorUrl)
                                                             .AddOrganizationUnitHeader(ExtAppModernFolderPaas_22_10.OrganizationUnitId),

        ModernFolderRobotHosts = new RobotHost[]
        {
            new RobotHost
            {
                MachineHostname = "ORCH-22-10-MODE",
                MachineUsername = "orch-22-10-mode\\uipathadmin",
            },
            new RobotHost
            {
                MachineHostname = "ORCH-22-10-2MOD",
                MachineUsername = "orch-22-10-2mod\\uipathadmin",
            },
        },

        SuccessfulTestSetPackageVersion = "1.0.171228354",
        SuccessfulTestCaseDefinitionId = 6,
        SuccessfulTestCaseReleaseId = 3,
        
        FailingTestSetPackageVersion = "1.0.171255661",
        FailingTestCaseDefinitionId = 274,
        FailingTestCaseReleaseId = 40486,
    };

    public static OrchestratorConnection ExtAppModernFolderPaas_23_4 { get; } = new OrchestratorConnection
    {
        ConnectionName = nameof(ExtAppModernFolderPaas_23_4),

        BaseUrl = "https://orch-23-4.westeurope.cloudapp.azure.com/",
        OrchestratorUrl = "https://orch-23-4.westeurope.cloudapp.azure.com/",
        AuthorizationUrl = "https://orch-23-4.westeurope.cloudapp.azure.com/identity",

        AccountForApp = null,
        OrchestratorTenant = "ciplugins",
        OrganizationUnit = "CIPlugins",
        OrganizationUnitId = "15",
        FolderFeedId = new Guid("ea048e10-567b-42a9-8906-0204dab99731"),

        ApplicationId = "44c99aa6-bdcb-4d7a-ac05-c0d78bf36bd9",
        ApplicationSecret = Environment.GetEnvironmentVariable(AgentEnvironment.Orchestrator_23_04_ExternalAppSecret),
        ApplicationScope = "OR.Folders OR.BackgroundTasks OR.TestSets OR.TestSetExecutions OR.TestSetSchedules OR.Settings.Read OR.Robots.Read OR.Machines.Read OR.Execution OR.Assets OR.Users.Read OR.Jobs OR.Monitoring",

        GetClientWithIdentityHandler = () => IdentityHandlers.Handlers.IdentityHandlers
                                                             .NewIdentityConfiguration(ExtAppModernFolderPaas_23_4.BaseUrl.AsIdentityServiceUrl(isMsiDeployment: true))
                                                             .WithExternalApp(
                                                                 ExtAppModernFolderPaas_23_4.ApplicationId,
                                                                 ExtAppModernFolderPaas_23_4.ApplicationSecret,
                                                                 ExtAppModernFolderPaas_23_4.ApplicationScope)
                                                             .ExternalAppTokenHandler()
                                                             .CreateHttpClient(baseAddress: ExtAppModernFolderPaas_23_4.OrchestratorUrl)
                                                             .AddOrganizationUnitHeader(ExtAppModernFolderPaas_23_4.OrganizationUnitId),

        ModernFolderRobotHosts = new RobotHost[]
        {
            new RobotHost
            {
                MachineHostname = "ORCH-23-4-MODER",
                MachineUsername = "orch-23-4-moder\\uipathadmin",
            },
            new RobotHost
            {
                MachineHostname = "ORCH-23-4-2MODE",
                MachineUsername = "orch-23-4-2mode\\uipathadmin",
            },
        },

        SuccessfulTestSetPackageVersion = "1.0.171228354",
        SuccessfulTestCaseDefinitionId = 10027,
        SuccessfulTestCaseReleaseId = 20008,
        
        FailingTestSetPackageVersion = "1.0.171255661",
        FailingTestCaseDefinitionId = 10368,
        FailingTestCaseReleaseId = 90623,
    };

    public static OrchestratorConnection ExtAppModernFolderCloud { get; } = new OrchestratorConnection
    {
        ConnectionName = nameof(ExtAppModernFolderCloud),

        BaseUrl = "https://cloud.uipath.com/",
        OrchestratorUrl = "https://cloud.uipath.com/studexhtdvnu/DefaultTenant/orchestrator_",

        AccountForApp = "studexhtdvnu",
        OrchestratorTenant = "DefaultTenant",
        OrganizationUnit = "E2eTests",
        OrganizationUnitId = "259463",
        FolderFeedId = new Guid("d655e5e3-ce7b-4fda-acee-9a2a1dceabe1"),

        ApplicationId = "71d75da6-4692-4132-9ffb-b6757082b300",
        ApplicationSecret = Environment.GetEnvironmentVariable(AgentEnvironment.CloudOrchestratorExternalAppSecret),
        ApplicationScope = "OR.Folders OR.BackgroundTasks OR.TestSets OR.TestSetExecutions OR.TestSetSchedules OR.Settings.Read OR.Robots.Read OR.Machines.Read OR.Execution OR.Assets OR.Users.Read OR.Jobs OR.Monitoring",

        GetClientWithIdentityHandler = () => IdentityHandlers.Handlers.IdentityHandlers
                                                             .NewIdentityConfiguration(ExtAppModernFolderCloud.BaseUrl.AsIdentityServiceUrl())
                                                             .WithExternalApp(
                                                                 ExtAppModernFolderCloud.ApplicationId,
                                                                 ExtAppModernFolderCloud.ApplicationSecret,
                                                                 ExtAppModernFolderCloud.ApplicationScope)
                                                             .ExternalAppTokenHandler()
                                                             .CreateHttpClient(baseAddress: ExtAppModernFolderCloud.OrchestratorUrl)
                                                             .AddOrganizationUnitHeader(ExtAppModernFolderCloud.OrganizationUnitId),

        ModernFolderRobotHosts = new RobotHost[]
        {
            new RobotHost
            {
                MachineHostname = "CLOUD-ROBOT-1",
                MachineUsername = "cloud-robot-1\\student",
            },
            new RobotHost
            {
                MachineHostname = "CLOUD-ROBOT-2",
                MachineUsername = "cloud-robot-2\\student",
            },
        },

        SuccessfulTestSetPackageVersion = "1.0.201541911",
        SuccessfulTestCaseDefinitionId = 10363,
        SuccessfulTestCaseReleaseId = 158154,
        
        FailingTestSetPackageVersion = "1.0.201542322",
        FailingTestCaseDefinitionId = 10364,
        FailingTestCaseReleaseId = 158162,
    };
}
