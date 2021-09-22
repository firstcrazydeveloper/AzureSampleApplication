namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using Microsoft.Azure.Cosmos;
    public class CosmosDbContainer : ICosmosDbContainer
    {
        public Container DbContainer { get; }

        public CosmosDbContainer(CosmosClient dbClient, string dbName, string containerName)
        {
            this.DbContainer = dbClient.GetContainer(dbName, containerName);
        }
    }
}
