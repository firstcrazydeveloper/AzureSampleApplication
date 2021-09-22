namespace SampleBlobTrigger.Application
{
    using Microsoft.Extensions.Logging;
    using Parquet;
    using Parquet.Data.Rows;
    using SampleBlobTrigger.Infrastructure.Persistence;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CandidateInsightService : ICandidateInsightService
    {
        private readonly ICandidateInsightRepository _candidateInsightRepository;
        private readonly ILogger<CandidateInsightService> _logger;
        private const int BulkPageSize = 100; // TODO move to config

        public CandidateInsightService(ICandidateInsightRepository candidateInsightRepository, ILogger<CandidateInsightService> logger)
        {
            _candidateInsightRepository = candidateInsightRepository;
            _logger = logger;
        }

        public async Task<bool> InsertDataToCosmosAsync(Stream blobContents, string fileName)
        {
            try
            {
                var parquetreader = new ParquetReader(blobContents);
                Table table = parquetreader.ReadAsTable();
                IDictionary<string, CandidateInsightDocument> candidateInsightDocuments = new Dictionary<string, CandidateInsightDocument>();
                IDictionary<string, string> partitionKeys = new Dictionary<string, string>();
                int temp = 0;
                foreach (var row in table)
                {
                    var candidateInsightDocument = GetCandidateInsightModel(row);
                    candidateInsightDocument.SiteKey = "54678";
                    candidateInsightDocument.InsightTextValue1 = "Testdata07";
                    candidateInsightDocuments.Add(candidateInsightDocument.Id, candidateInsightDocument);
                    partitionKeys.Add(candidateInsightDocument.Id, candidateInsightDocument.SiteKey);
                    temp++;
                }

                await InsertBulkDataToCosmos(candidateInsightDocuments, partitionKeys);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        private async Task<bool> InsertBulkDataToCosmos(IDictionary<string, CandidateInsightDocument> values, IDictionary<string, string> partitionKeys)
        {
            int currentPage = 1;
            bool canPage = true;

            while (canPage)
            {
                int skip = BulkPageSize * (currentPage - 1);
                var filtersRecords = values.Select(record => record)
                    .Skip(skip)
                    .Take(BulkPageSize)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                currentPage++;
                canPage = (skip + BulkPageSize) < values.Count();
                await _candidateInsightRepository.InsertBulkData(filtersRecords, partitionKeys);
            }

            return true;

        }

        private CandidateInsightDocument GetCandidateInsightModel(Row rowdata)
        {
            try
            {
                CandidateInsightDocument dbDocument = new CandidateInsightDocument();
                dbDocument.Id = Guid.NewGuid().ToString();
                dbDocument.CandidateInsightKey = GetStringFromRow(rowdata, 18);
                dbDocument.InsightDescriptionID = GetStringFromRow(rowdata, 12);
                dbDocument.DeviceId = GetStringFromRow(rowdata, 0);
                dbDocument.SiteKey = GetStringFromRow(rowdata, 5);
                dbDocument.AccountNumber = GetStringFromRow(rowdata, 7);
                dbDocument.ServiceArea = GetStringFromRow(rowdata, 8);
                dbDocument.ServiceAreaId = GetStringFromRow(rowdata, 8);
                dbDocument.AlertTime = GetStringFromRow(rowdata, 2);
                dbDocument.Shift = GetStringFromRow(rowdata, 4);
                dbDocument.InsightCategory = GetStringFromRow(rowdata, 15);
                dbDocument.InsightCreationTimestamp = GetStringFromRow(rowdata, 10);
                dbDocument.InsightTextValue1 = GetStringFromRow(rowdata, 20, 0);
                dbDocument.InsightTextValue2 = GetStringFromRow(rowdata, 20, 1);

                return dbDocument;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private string GetStringFromRow(Row rowData, int firstIndex, int secondIndex)
        {
            if (rowData.Values.Length < firstIndex || ((object[])rowData.Values[firstIndex]).Length < secondIndex)
            {
                throw new IndexOutOfRangeException();
            }

            return ((object[])rowData.Values[firstIndex])[secondIndex] == null ? "" : ((object[])rowData.Values[firstIndex])[secondIndex].ToString(); ;
        }

        private string GetStringFromRow(Row rowData, int index)
        {
            if (rowData.Values.Length < index)
            {
                throw new IndexOutOfRangeException();
            }

            return rowData.Values[index] == null ? "" : rowData.Values[index].ToString();
        }

    }
}
