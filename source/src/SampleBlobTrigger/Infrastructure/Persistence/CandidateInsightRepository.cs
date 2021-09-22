namespace SampleBlobTrigger.Infrastructure.Persistence
{
    using EnsureThat;
    using Microsoft.Extensions.Logging;
    using SampleBlobTrigger.Infrastructure.Persistence.Common;
    using System;
    using System.Collections.Generic;
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

        public Task AddCandidateInsightDataAsync(CandidateInsightDocument document)
        {
            try
            {
                EnsureArg.IsNotNull(document, nameof(document));

                 return _documentDb.CreateAsync(document, document.SiteKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> InsertBulkData(IDictionary<string, CandidateInsightDocument> values, IDictionary<string, string> partitionKeys)
        {
            try
            {
                EnsureArg.IsNotNull(values, nameof(values));
                EnsureArg.IsNotNull(partitionKeys, nameof(partitionKeys));

                await _documentDb.InsertBulkData(values, partitionKeys);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
