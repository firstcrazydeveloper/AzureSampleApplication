namespace SampleBlobTrigger.Extensions
{
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.DependencyInjection;
    using SampleBlobTrigger.Infrastructure.Persistence.Common;
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, string connectionString, string databaseName)
        {
            CosmosClientOptions options = new CosmosClientOptions() { AllowBulkExecution = true };
            options.MaxRetryAttemptsOnRateLimitedRequests = 30;
            var client = new CosmosClient(connectionString, options);
            CosmosDbContainerFactory cosmosDbContainerFactory = new CosmosDbContainerFactory(client, databaseName);
            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbContainerFactory);
            return services;
        }
    }
}
