namespace SampleBlobTrigger.Infrastructure.Persistence
{
    using System.Threading.Tasks;

    public interface ICandidateInsightRepository
    {
        Task<CandidateInsightDocument> AddCandidateInsightDataAsync(CandidateInsightDocument document);
    }
}
