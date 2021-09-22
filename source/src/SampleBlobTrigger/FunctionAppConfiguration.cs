namespace FunctionAppBlobStorageTrigger
{
    using System;

    public class FunctionAppConfiguration : IFunctionAppConfiguration
    {
        public string CosmosDatabaseName
        {
            get
            {
                return "apidatabasetest";
            }
        }

        public string CosmosCollectionName
        {
            get
            {
                return "MasteredInsights";
            }
        }

        public string CosmosConnectionString
        {
            get
            {
                return "AccountEndpoint=https://ecolab3dcosmos.documents.azure.com:443/;AccountKey=ud4wAL52019dxVh55SsMHdj770txIoKukAHWqkJK97okLOiNciay6R1BpCCHZ0NPVvlTctmzR3TLTmQsqmkaHQ==;";
            }
        }
    }
}
