namespace SampleBlobTrigger.Application
{
    using Microsoft.Extensions.Logging;
    using Parquet;
    using Parquet.Data.Rows;
    using SampleBlobTrigger.Infrastructure;
    using SampleBlobTrigger.Infrastructure.Persistence;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class CandidateInsightService : ICandidateInsightService
    {
        private readonly ICandidateInsightRepository _candidateInsightRepository;
        private readonly ILogger<CandidateInsightService> _logger;

        public CandidateInsightService(ICandidateInsightRepository candidateInsightRepository, ILogger<CandidateInsightService> logger)
        {
            _candidateInsightRepository = candidateInsightRepository;
            _logger = logger;
        }

        public async Task InsertDataToCosmosAsync(Stream blobContents, string fileName)
        {
            try
            {
                var parquetreader = new ParquetReader(blobContents);
                Table table = parquetreader.ReadAsTable();
                var candidateInsightDocuments = new List<CandidateInsightDocument>();
                foreach (Row row in table)
                {
                    var candidateInsightDocument = GetCandidateInsightModel(row);
                    candidateInsightDocuments.Add(candidateInsightDocument);
                }
            }
            catch (Exception ex)
            {

            }

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
            if (rowData.Values.Length > firstIndex || ((string[])rowData.Values[firstIndex]).Length > secondIndex)
            {
                throw new IndexOutOfRangeException();
            }

            return ((string[])rowData.Values[firstIndex])[secondIndex] == null ? "" : ((string[])rowData.Values[firstIndex])[secondIndex].ToString(); ;
        }

        private string GetStringFromRow(Row rowData, int index)
        {
            if (rowData.Values.Length > index)
            {
                throw new IndexOutOfRangeException();
            }

            return rowData.Values[index] == null ? "" : rowData.Values[index].ToString();
        }

    }
}
