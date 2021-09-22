namespace FunctionAppBlobStorageTrigger.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using SampleBlobTrigger.Application;

    public class ReadBlobStorageFileFunction
    {
        private readonly ICandidateInsightService _candidateInsightService;
        private readonly ILogger<ReadBlobStorageFileFunction> _logger;
        public ReadBlobStorageFileFunction(ICandidateInsightService candidateInsightService, ILogger<ReadBlobStorageFileFunction> logger)
        {
            _candidateInsightService = candidateInsightService;
            _logger = logger;
        }

        [FunctionName("ReadFileFromBlobStorage")]
        public async Task ReadFileFromBlobStorage([BlobTrigger("%BlobContainerName%/{name}", Connection = "BlobConnectionString")]
                                                        Stream blobFileContents, string blobTrigger, string name)
        {
            try
            {
                if (name.Contains(".parquet") && name.StartsWith("Final_Insights34"))
                {
                    await _candidateInsightService.InsertDataToCosmosAsync(blobFileContents, name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
    }
}
