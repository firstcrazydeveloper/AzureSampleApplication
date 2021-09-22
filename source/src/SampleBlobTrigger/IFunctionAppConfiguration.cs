namespace FunctionAppBlobStorageTrigger
{
    public interface IFunctionAppConfiguration
    {
        string CosmosDatabaseName { get; }

        string CosmosCollectionName { get;  }

        string CosmosConnectionString { get; }

    }
}
