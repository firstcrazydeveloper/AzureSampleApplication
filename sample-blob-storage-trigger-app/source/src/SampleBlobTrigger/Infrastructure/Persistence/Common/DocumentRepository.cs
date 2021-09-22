namespace SampleBlobTrigger.Infrastructure.Persistence.Common
{
    using FunctionAppBlobStorageTrigger;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Logging;
    using Polly;
    using System;
    using System.Threading.Tasks;
    using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

    public class DocumentRepository<T> : IDocumentRepository<T>
        where T : class
    {
        private readonly ILogger<DocumentRepository<T>> _logger;
        private readonly IAsyncPolicy _retryPolicy;
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly IFunctionAppConfiguration _configuration;
        private readonly Container _container;

        public DocumentRepository(ICosmosDbContainerFactory cosmosDbContainerFactory, IFunctionAppConfiguration configuration, ILogger<DocumentRepository<T>> logger)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory;
            _logger = logger;
            _configuration = configuration;
            var maxRetryAttempt = 5; //Todo move to configuration
            var pauseIntervalTimeBetweenFailures = 5; //Todo move to configuration
            _container = _cosmosDbContainerFactory.GetContainer(_configuration.CosmosCollectionName).DbContainer;

            _retryPolicy = Policy
                .Handle<System.Net.Sockets.SocketException>()
                .Or<System.Net.Http.HttpRequestException>()
                .Or<DocumentClientException>(_ => _.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                .Or<DocumentClientException>(_ => _.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .Or<OperationCanceledException>()
                .WaitAndRetryAsync(
                maxRetryAttempt,
                _ => TimeSpan.FromSeconds(pauseIntervalTimeBetweenFailures),
                (exception, _, retryCount, context) =>
                {
                    _logger.LogError(exception, exception.Message);
                });

        }

        public async Task<T> CreateAsync(T value, string partitionKey)
        {
            try
            {
                var createResult = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var document = await _container.CreateItemAsync(value, new PartitionKey(partitionKey)).ConfigureAwait(false);
                    return document;

                }).ConfigureAwait(false);

                return createResult?.Resource;
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
    }
}
