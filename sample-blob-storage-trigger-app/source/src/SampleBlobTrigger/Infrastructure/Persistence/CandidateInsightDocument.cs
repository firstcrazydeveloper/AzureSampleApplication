namespace SampleBlobTrigger.Infrastructure.Persistence
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    public class CandidateInsightDocument
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }

        [DataMember]
        [JsonProperty("candidateInsightKey")]
        public string CandidateInsightKey { get; set; }

        [DataMember]
        [JsonProperty("insightDescriptionID")]
        public string InsightDescriptionID { get; set; }

        [DataMember]
        [JsonProperty("deviceId")] 
        public string DeviceId { get; set; }

        [DataMember]
        [JsonProperty("siteKey")] 
        public string SiteKey { get; set; }

        [DataMember]
        [JsonProperty("accountNumber")] 
        public string AccountNumber { get; set; }

        [DataMember]
        [JsonProperty("serviceArea")] 
        public string ServiceArea { get; set; }

        [DataMember]
        [JsonProperty("serviceAreaId")] 
        public string ServiceAreaId { get; set; }

        [DataMember]
        [JsonProperty("alertTime")]

        public string AlertTime { get; set; }

        [DataMember]
        [JsonProperty("shift")]
        public string Shift { get; set; }

        [DataMember]
        [JsonProperty("insightCategory")] 
        public string InsightCategory { get; set; }

        [DataMember]
        [JsonProperty("insightCreationTimestamp")] 
        public string InsightCreationTimestamp { get; set; }

        [DataMember]
        [JsonProperty("insightTextValue1")] 
        public string InsightTextValue1 { get; set; }

        [DataMember]
        [JsonProperty("insightTextValue2")] 
        public string InsightTextValue2 { get; set; }
    }
}
