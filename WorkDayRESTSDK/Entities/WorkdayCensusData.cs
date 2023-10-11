using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkDayRESTSDK.Entities
{
    public class WorkdayCensusData
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }

        [JsonProperty("wd:Report_Data")]
        public WdReportData WdReportData { get; set; }
    }

    public class WdPayRateType
    {
        [JsonProperty("@wd:Descriptor")]
        public string WdDescriptor { get; set; }

        [JsonProperty("wd:ID")]
        public List<WdID> WdID { get; set; }
    }

    public class WdPosition
    {
        [JsonProperty("@wd:Descriptor")]
        public string WdDescriptor { get; set; }

        [JsonProperty("wd:ID")]
        public WdID WdID { get; set; }
    }

    public class WdTotalBasePayFrequency
    {
        [JsonProperty("@wd:Descriptor")]
        public string WdDescriptor { get; set; }

        [JsonProperty("wd:ID")]
        public List<WdID> WdID { get; set; }
    }

}
