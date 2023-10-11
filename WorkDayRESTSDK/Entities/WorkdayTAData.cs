using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkDayRESTSDK.Entities
{
    public class WorkdayTAData
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }

        [JsonProperty("wd:Report_Data")]
        public WdReportData WdReportData { get; set; }
    }

    public class WdWorker
    {
        [JsonProperty("@wd:Descriptor")]
        public string WdDescriptor { get; set; }

        [JsonProperty("wd:ID")]
        public List<WdID> WdID { get; set; }

    }

}
