namespace SampleBlobTrigger.Infrastructure.Persistence
{
    using EnsureThat;
    using Microsoft.Extensions.Logging;
    using SampleBlobTrigger.Infrastructure.Persistence.Common;
    using System;
    using System.Threading.Tasks;

    public class CandidateInsightRepository : ICandidateInsightRepository
    {
        private readonly ILogger<CandidateInsightRepository> _logger;
        private readonly IDocumentRepository<CandidateInsightDocument> _documentDb;

        public CandidateInsightRepository(IDocumentRepository<CandidateInsightDocument> documentDb, ILogger<CandidateInsightRepository> logger)
        {
            _documentDb = documentDb;
            _logger = logger;
        }

        public async Task<CandidateInsightDocument> AddCandidateInsightDataAsync(CandidateInsightDocument document)
        {
            try
            {
                EnsureArg.IsNotNull(document, nameof(document));

                var responseData = await _documentDb.CreateAsync(document, document.SiteKey).ConfigureAwait(false);
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
