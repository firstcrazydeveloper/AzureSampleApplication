using FunctionAppBlobStorageTrigger;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Startup))]

namespace FunctionAppBlobStorageTrigger
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using SampleBlobTrigger.Application;
    using SampleBlobTrigger.Extensions;
    using SampleBlobTrigger.Infrastructure.Persistence;
    using SampleBlobTrigger.Infrastructure.Persistence.Common;

    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            RegisterServices(builder.Services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IFunctionAppConfiguration, FunctionAppConfiguration>();
            var config = services.BuildServiceProvider().GetRequiredService<IFunctionAppConfiguration>();
            services.AddCosmosDb(config.CosmosConnectionString, config.CosmosDatabaseName);
            services.AddSingleton<ICandidateInsightRepository, CandidateInsightRepository>();
            services.AddTransient(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));
            services.AddSingleton<ICandidateInsightService, CandidateInsightService>();
        }
    }
}
