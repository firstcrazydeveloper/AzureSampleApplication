namespace FunctionAppBlobStorageTrigger.Functions
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using SampleBlobTrigger.Application;

    public class ReadBlobStorageFile
    {
        private readonly ICandidateInsightService _candidateInsightService;
        public ReadBlobStorageFile(ICandidateInsightService candidateInsightService)
        {
            _candidateInsightService = candidateInsightService;
        }

        [FunctionName("GetFileFromBlobStorage")]
        public async Task GetFileFromBlobStorage([BlobTrigger("%BlobTriggerName%/{name}", Connection = "BlobConnectionString")]
                                                        Stream blobFileContents, string blobTrigger, string name)
        {
            await _candidateInsightService.InsertDataToCosmosAsync(blobFileContents, name).ConfigureAwait(false);

        }
    }
}
