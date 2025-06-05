using System.Text;
using Microsoft.Rest;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;
using UiPath.Extensions.CommandLine.E2E.Tests.Extensions;
using UiPath.Extensions.CommandLine.E2E.Tests.OrchestratorConnections;
using UiPath.Extensions.CommandLine.E2E.Tests.TestData;
using UiPath.Orchestrator.Web.ClientV3;

namespace UiPath.Extensions.CommandLine.E2E.Tests;

public class ManageAssetsShould
{
    [Theory]
    [ExtAppModernFolderConnections]
    [ExtAppModernFolderConnectionsWithInlineArgsCli]
    [Trait(TraitConsts.Agent, TraitConsts.Linux)]
    public async Task CreateUpdateAndDeleteAssets(CliExecutor cliExecutor, OrchestratorConnection connection)
    {
        const string integerAssetType = "integer";
        const string credentialAssetType = "credential";
        const string booleanAssetType = "bool";
        const string textAssetType = "text";

        var integerAssetName = Guid.NewGuid().ToString();
        var integerAssetCreateValue = 1;
        var integerAssetUpdateValue = 11;

        var credentialAssetName = Guid.NewGuid().ToString();
        var credentialAssetCreateValue = "username1::password1";

        var booleanAssetName = Guid.NewGuid().ToString();
        var booleanAssetCreateValue = "true";
        
        var textAssetName = Guid.NewGuid().ToString();
        var textAssetCreateValue = "it works!";

        var firstDeployment = new Dictionary<string, (string, object)>
        {
            { integerAssetName, (integerAssetType, integerAssetCreateValue) },
            { credentialAssetName, (credentialAssetType, credentialAssetCreateValue) }
        };

        var secondDeployment = new Dictionary<string, (string, object)>
        {
            { integerAssetName, (integerAssetType, integerAssetUpdateValue) },
            { booleanAssetName, (booleanAssetType, booleanAssetCreateValue) },
            { textAssetName, (textAssetType, textAssetCreateValue) }
        };

        var firstDelete = new List<string> { booleanAssetName, textAssetName };

        var httpClient = connection.GetClientWithIdentityHandler();
        var assetsClient = new AssetsClient(new(), httpClient);
        

        var firstDeploymentFilePath = Common.Utils.CreateDeployAssetsFile(firstDeployment);
        await cliExecutor.ManageAssets<DeployAssetsOptions>(connection, firstDeploymentFilePath);

        var assetsToFetch = new List<(string, string)> { 
            (integerAssetName, integerAssetType),
            (credentialAssetName, credentialAssetType)
        };
        var assets = await GetAssetsByNamesAndAssertFetchedNumber(assetsClient, assetsToFetch);
        var actualIntegerAsset = assets.First(asset => asset.Name.Equals(integerAssetName));
        Assert.Equal(integerAssetCreateValue.ToString(), actualIntegerAsset.Value);


        var secondDeploymentFilePath = Common.Utils.CreateDeployAssetsFile(secondDeployment);
        await cliExecutor.ManageAssets<DeployAssetsOptions>(connection, secondDeploymentFilePath);

        assetsToFetch = new List<(string, string)> {
            (integerAssetName, integerAssetType),
            (booleanAssetName, booleanAssetType),
            (textAssetName, textAssetType)
        };
        assets = await GetAssetsByNamesAndAssertFetchedNumber(assetsClient, assetsToFetch);
        actualIntegerAsset = assets.First(asset => asset.Name.Equals(integerAssetName));
        Assert.Equal(integerAssetUpdateValue.ToString(), actualIntegerAsset.Value);


        var firstDeleteFilePath = CreateDeleteAssetsFile(firstDelete);
        await cliExecutor.ManageAssets<DeleteAssetsOptions>(connection, firstDeleteFilePath);

        assetsToFetch = new List<(string, string)> {
            (booleanAssetName, booleanAssetType),
            (textAssetName, textAssetType)
        };
        await GetAssetsByNamesAndAssertFetchedNumber(assetsClient, assetsToFetch, expectedNumberOfFetchedAssets: 0);


        var secondDeleteFilePath = firstDeploymentFilePath;
        await cliExecutor.ManageAssets<DeleteAssetsOptions>(connection, secondDeleteFilePath);

        assetsToFetch = new List<(string, string)> {
            (integerAssetName, integerAssetType),
            (credentialAssetName, credentialAssetType)
        };
        await GetAssetsByNamesAndAssertFetchedNumber(assetsClient, assetsToFetch, expectedNumberOfFetchedAssets: 0);
    }

    private static string CreateDeleteAssetsFile(List<string> assetsNames)
    {
        var content = new StringBuilder();
        content.AppendLine("name");

        foreach (var name in assetsNames)
        {
            content.AppendLine(name);
        };

        var deleteAssetsPath = Common.Utils.GetRandomCsvFileInTempPath();
        File.WriteAllText(deleteAssetsPath, content.ToString());

        return deleteAssetsPath;
    }

    private static async Task<IEnumerable<AssetDto>> GetAssetsByNamesAndAssertFetchedNumber(AssetsClient assetsClient, IEnumerable<(string Name, string Type)> assets, int? expectedNumberOfFetchedAssets = null)
    {
        var names = assets.Select(asset => asset.Name);

        var filterList = "(" + string.Join(", ", names.Select(name => $"'{name}'")) + ")";
        var filter = $"Name in {filterList}";

        IEnumerable<AssetDto> fetchedAssets;
        try
        {
            var assetsResponse = await assetsClient.GetFilteredAsync(filter: filter);
            fetchedAssets = assetsResponse.Body.Value;
        } catch (HttpOperationException)
        {            
            fetchedAssets = await GetAssestsByNamesInOldOrchestrator(assetsClient, names);
        }

        if (expectedNumberOfFetchedAssets is not null)
        {
            Assert.Equal(expectedNumberOfFetchedAssets, fetchedAssets.Count());
            return fetchedAssets;
        }
            
        Assert.Equal(assets.Count(), fetchedAssets.Count());
        foreach (var asset in assets)
        {
            Assert.Contains(fetchedAssets, fetchedAsset => fetchedAsset.Name.Equals(asset.Name) && fetchedAsset.ValueType.ToString().ToLower().Equals(asset.Type));
        }

        return fetchedAssets;
    }

    private static async Task<IEnumerable<AssetDto>> GetAssestsByNamesInOldOrchestrator(AssetsClient assetsClient, IEnumerable<string> names)
    {
        var assetsList = new List<AssetDto>();
        foreach (var name in names)
        {
            var filter = $"Name eq '{name}'";
            var assetsResponse = await assetsClient.GetAsync(filter: filter);
            var fetchedAssets = assetsResponse.Body.Value;
            var asset = fetchedAssets.FirstOrDefault();
            if (asset is not null)
                assetsList.Add(asset);
        }

        return assetsList;
    }
}
