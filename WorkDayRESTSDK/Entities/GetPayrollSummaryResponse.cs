using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WorkDayRESTSDK.Entities
{
    public class WdGetPayrollSummaryResponse
    {
        public int? CurrPage { get; set; }
        public int? PageSize { get; set; }
        public int? TotalPages { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public List<WdEmployeePayrollSummary> EmployeePayrollSummaries { get; set; }
    }

    public class WdEmployeePayrollSummary
    {
        public string EmployeeID { get; set; }
        public double GrossAmount { get; set; }
        public double NetAmount { get; set; }
    }
}
