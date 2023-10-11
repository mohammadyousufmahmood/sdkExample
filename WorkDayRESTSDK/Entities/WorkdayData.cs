using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkDayRESTSDK.Entities
{
    public class WdID
    {
        [JsonProperty("@wd:type")]
        public string WdType { get; set; }

        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class WdReportData
    {
        [JsonProperty("@xmlns:wd")]
        public string XmlnsWd { get; set; }

        [JsonProperty("wd:Report_Entry")]
        public List<WdReportEntry> WdReportEntry { get; set; }
    }

    public class WdReportEntry
    {
        // Census

        [JsonProperty("wd:Employee_ID")]
        public string WdEmployeeID { get; set; }

        [JsonProperty("wd:First_Name")]
        public string WdFirstName { get; set; }

        [JsonProperty("wd:Last_Name")]
        public string WdLastName { get; set; }

        [JsonProperty("wd:Hire_Date")]
        public string WdHireDate { get; set; }

        [JsonProperty("wd:Work_State")]
        public string WdWorkState { get; set; }

        [JsonProperty("wd:Pay_Rate_Type")]
        public WdPayRateType WdPayRateType { get; set; }

        [JsonProperty("wd:Location_Name")]
        public string WdLocationName { get; set; }

        [JsonProperty("wd:Location_ID")]
        public string WdLocationID { get; set; }

        [JsonProperty("wd:Total_Base_Pay_Annualized_-_Amount")]
        public string WdTotalBasePayAnnualizedAmount { get; set; }

        [JsonProperty("wd:Total_Base_Pay_-_Hourly")]
        public string WdTotalBasePayHourly { get; set; }
        [JsonProperty("wd:Exempt")]
        public string WdExempt { get; set; }

        [JsonProperty("wd:Total_Base_Pay_-_Amount")]
        public string WdTotalBasePayAmount { get; set; }

        [JsonProperty("wd:Total_Base_Pay_-_Frequency")]
        public WdTotalBasePayFrequency WdTotalBasePayFrequency { get; set; }

        [JsonProperty("wd:Active_Status")]
        public string WdActiveStatus { get; set; }

        [JsonProperty("wd:Terminated")]
        public string WdTerminated { get; set; }

        [JsonProperty("wd:Termination_Date")]
        public string WdTerminationDate { get; set; }

        [JsonProperty("wd:On_Leave")]
        public string WdOnLeave { get; set; }

        [JsonProperty("wd:Position")]
        public WdPosition WdPosition { get; set; }

        //TA

        [JsonProperty("wd:Worker")]
        public WdWorker WdWorker { get; set; }

        [JsonProperty("wd:Date")]
        public string WdDate { get; set; }

        [JsonProperty("wd:Hours")]
        public string WdHours { get; set; }

        [JsonProperty("wd:Is_Approved")]
        public string WdIsApproved { get; set; }

        [JsonProperty("wd:Approval_Date")]
        public DateTime WdApprovalDate { get; set; }

        [JsonProperty("wd:Last_four_SSN")]
        public string wdLastfourSSN { get; set; }
        [JsonProperty("wd:Time_Entry_Code")]
        public WdWorker wdTimeEntryCode { get; set; }
   

        [JsonProperty("wd:Email")]
        public string wdEmail { get; set; }
        
        [JsonProperty("wd:Pay_Group")]
        public string wdPayGroup { get; set; }
    }
    public class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@encoding")]
        public string Encoding { get; set; }
    }


}
