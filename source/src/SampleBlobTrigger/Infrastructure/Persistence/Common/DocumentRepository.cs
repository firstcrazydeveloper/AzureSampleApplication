namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using FunctionAppBlobStorageTrigger;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Documents;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

    public class DocumentRepository<T> : IDocumentRepository<T>
        where T : class
    {
        private const int BulkPageSize = 100; // TODO move to config
        private readonly ILogger<DocumentRepository<T>> _logger;
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly IFunctionAppConfiguration _configuration;
        private readonly Container _container;

        public DocumentRepository(ICosmosDbContainerFactory cosmosDbContainerFactory, IFunctionAppConfiguration configuration, ILogger<DocumentRepository<T>> logger)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory;
            _logger = logger;
            _configuration = configuration;
            _container = _cosmosDbContainerFactory.GetContainer(_configuration.CosmosCollectionName).DbContainer;
        }

        public Task CreateAsync(T value, string partitionKey)
        {
            try
            {
              var response =   _container.CreateItemAsync(value, new PartitionKey(partitionKey)).ContinueWith(itemResponse =>
                {
                    if (!itemResponse.IsCompletedSuccessfully)
                    {
                        AggregateException innerExceptions = itemResponse.Exception.Flatten();
                        if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                        {
                            _logger.LogTrace($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                        }
                        else
                        {
                            _logger.LogTrace($"Exception {innerExceptions.Flatten().ToString()}.");
                        }
                    }
                });

                return response;
            }
            catch(DocumentClientException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task<bool> InsertBulkData(IDictionary<string, T> values, IDictionary<string, string> partitionKeys)
        {
            IList<Task> concurrentTasks = new List<Task>();
            foreach (var item in values)
            {
                
                concurrentTasks.Add(_container.CreateItemAsync(values[item.Key], new PartitionKey(partitionKeys[item.Key])).ContinueWith(itemResponse =>
                {
                    if (!itemResponse.IsCompletedSuccessfully)
                    {
                        AggregateException innerExceptions = itemResponse.Exception.Flatten();
                        if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                        {
                            _logger.LogTrace($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                        }
                        else
                        {
                            _logger.LogTrace($"Exception {innerExceptions.Flatten().ToString()}.");
                        }
                    }
                }));
            }

            await Task.WhenAll(concurrentTasks);
            return true;
        }
    }
}
