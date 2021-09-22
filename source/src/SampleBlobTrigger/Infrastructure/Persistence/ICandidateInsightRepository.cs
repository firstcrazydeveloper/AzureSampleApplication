namespace SampleBlobTrigger.Infrastructure.Persistence
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICandidateInsightRepository
    {
        Task AddCandidateInsightDataAsync(CandidateInsightDocument document);

        Task<bool> InsertBulkData(IDictionary<string, CandidateInsightDocument> values, IDictionary<string, string> partitionKeys);
    }
}
