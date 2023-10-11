using System;
using System.Collections.Generic;
using System.Text;

namespace WorkDayRESTSDK.Entities
{
    public class WorkDayAdditionalSettings
    {
        public string BaseURL { get; set; }
        public string Endpoint { get; set; }
        public string CensusEndpoint { get; set; }
        public string CensusURL { get; set; }
        public string TAURL { get; set; }
        public string PayrollSummaryUrl { get; set; }
        public string TAEndpoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Census_MapPayGroupAsDepCode{ get; set; }
        public bool? Emp_DeactivateOnLeave { get; set; }
        public string Census_WorkdayHybridStates { get; set; }
        public string Census_WorkdayHybridDepartmentContateString { get; set; }
        public string Census_InformationToHide_csv { get; set; }
        public decimal? DefaultTaxDeductionPercentageAll { get; set; }


    }
}
