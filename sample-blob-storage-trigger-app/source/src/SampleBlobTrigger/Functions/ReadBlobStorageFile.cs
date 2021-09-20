namespace FunctionAppBlobStorageTrigger.Functions
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;

    public class ReadBlobStorageFile
    {
        public ReadBlobStorageFile()
        {

        }

        [FunctionName("GetFileFromBlobStorage")]
        public async Task GetFileFromBlobStorage([BlobTrigger("%BlobTriggerName%/{name}", Connection = "BlobConnectionString")]
                                                        Stream blobFileContents, string blobTrigger, string name)
        {
            await Task.Run(() => Play());

        }

        private void Play()
        {

        }
    }
}
