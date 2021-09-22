namespace SampleBlobTrigger.Application
{
    using System.IO;
    using System.Threading.Tasks;

    public interface ICandidateInsightService
    {
        Task<bool> InsertDataToCosmosAsync(Stream blobContents, string fileName);
    }
}
