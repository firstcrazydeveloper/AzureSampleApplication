namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Management.AppService.Fluent.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class CosmosDbContainerFactory : ICosmosDbContainerFactory
    {
        private readonly CosmosClient _dbClient;
        private readonly string _dbName;
        public CosmosDbContainerFactory(CosmosClient dbClient, string dbName)
        {
            _dbClient = dbClient ?? throw new ArgumentNullException(nameof(dbClient));
            _dbName = dbName ?? throw new ArgumentNullException(nameof(dbName));
        }
        public ICosmosDbContainer GetContainer(string containerName)
        {
            return new CosmosDbContainer(_dbClient, _dbName, containerName);
        }
    }
}
