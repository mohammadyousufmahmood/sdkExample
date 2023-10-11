using HRISGatewayInterface.Entities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WorkDayRESTSDK.Entities;

namespace WorkDayRESTSDK
{
    public class WorkDayRESTClient
    {
        WorkDayAdditionalSettings workDayAdditionalSettings = null;
        RestClient client = null;
        HttpClient httpClient = null;
        public WorkDayRESTClient(WorkDayAdditionalSettings workDayAdditionalSettings)
        {
            if (workDayAdditionalSettings == null) throw new Exception("Wrokday Additional settings cannot be null!");
            if (string.IsNullOrWhiteSpace(workDayAdditionalSettings.BaseURL)) throw new Exception("Invalid URL!");
            if (string.IsNullOrWhiteSpace(workDayAdditionalSettings.Username)) throw new Exception("Invalid Username!");
            if (string.IsNullOrWhiteSpace(workDayAdditionalSettings.Password)) throw new Exception("Invalid Password!");
            if (string.IsNullOrWhiteSpace(workDayAdditionalSettings.PayrollSummaryUrl)) throw new Exception("Invalid PayrollSummaryUrl!");

            client = new RestClient();
            httpClient = new HttpClient(new HttpClientHandler() { UseCookies = false });
            client.Timeout = -1;
            this.workDayAdditionalSettings = workDayAdditionalSettings;

        }

        #region Census

        public async Task<HRISGatewayInterface.Entities.Response<IEnumerable<EmployeeDetail>>> GetCensus()
        {
            var responseEmployeeDetail = new HRISGatewayInterface.Entities.Response<IEnumerable<EmployeeDetail>>();
            var employeeDetailList = new List<EmployeeDetail>();
            decimal hourlyRate = 0;

            try
            {
                if ((string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusEndpoint) || string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusEndpoint)) && string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusURL)) throw new ArgumentNullException("Missing URL for census Syncing");
                if (!string.IsNullOrWhiteSpace(workDayAdditionalSettings.BaseURL) && !workDayAdditionalSettings.BaseURL.LastOrDefault().Equals('/')) workDayAdditionalSettings.BaseURL += "/";
                if (!string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusEndpoint) && !workDayAdditionalSettings.CensusEndpoint.FirstOrDefault().Equals('/')) workDayAdditionalSettings.CensusEndpoint = "/" + workDayAdditionalSettings.CensusEndpoint;

                var url = workDayAdditionalSettings.CensusURL ?? (workDayAdditionalSettings.BaseURL + workDayAdditionalSettings.Username.Split('@')[0] + workDayAdditionalSettings.CensusEndpoint);

                var censusResponse = (await ExecuteRequest(url)).Data;
                if (!string.IsNullOrWhiteSpace(censusResponse))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(censusResponse);
                    var jsonText = JsonConvert.SerializeXmlNode(doc);
                    var censusData = JsonConvert.DeserializeObject<WorkdayCensusData>(jsonText, (new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                    var jsonstring = JsonConvert.SerializeObject(censusData.WdReportData.WdReportEntry.FirstOrDefault());

                    if (censusData.WdReportData != null && censusData.WdReportData.WdReportEntry.Count() > 0 && censusData.WdReportData.WdReportEntry.Any())
                    {
                        foreach (var emp in censusData.WdReportData.WdReportEntry)
                        {
                            EmployeeStatus empStatus = emp.WdActiveStatus == "1" ? EmployeeStatus.Active : (emp.WdTerminated == "1" ? EmployeeStatus.Terminated : EmployeeStatus.Deactivated);
                            empStatus = workDayAdditionalSettings.Emp_DeactivateOnLeave != false && emp.WdOnLeave == "1" ? EmployeeStatus.Deactivated : empStatus;
                            DateTime termDate = string.IsNullOrEmpty(emp.WdTerminationDate) ? DateTime.MinValue : Convert.ToDateTime(emp.WdTerminationDate);
                            ContractType contractType = ContractType.FullTimeSalaried;

                            if (emp.WdExempt != null)
                            {
                                contractType = emp.WdExempt == "0" ? ContractType.FullTimeHourly : ContractType.FullTimeSalaried;
                            }
                            else
                            {
                                try
                                {
                                    contractType = emp.WdPayRateType.WdDescriptor.Equals("Hourly", StringComparison.InvariantCultureIgnoreCase) ? ContractType.FullTimeHourly : ContractType.FullTimeSalaried;
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            if (emp.WdTotalBasePayHourly != null)
                            {
                                hourlyRate = Convert.ToDecimal(emp.WdTotalBasePayHourly);
                            }
                            else
                            {
                                var payRateFrequencyType = emp?.WdTotalBasePayFrequency?.WdDescriptor ?? emp?.WdPayRateType?.WdDescriptor ?? "hourly";
                                //var hourlyRate = contractType == ContractType.FullTimeSalaried ? Convert.ToDecimal(emp.WdTotalBasePayAmount) / 2080 : Convert.ToDecimal(emp.WdTotalBasePayAmount);
                                hourlyRate = !payRateFrequencyType.Equals("Hourly", StringComparison.InvariantCultureIgnoreCase) ? Convert.ToDecimal(emp.WdTotalBasePayAmount) / 2080 : Convert.ToDecimal(emp.WdTotalBasePayAmount);
                            }
                            EmployeeDetail employeeDetail = new EmployeeDetail();
                            employeeDetail.InternalEmployeeID = emp.WdEmployeeID;
                            employeeDetail.EmployerGivenID = emp.WdEmployeeID;
                            employeeDetail.FirstName = emp.WdFirstName;
                            employeeDetail.LastName = emp.WdLastName;
                            employeeDetail.ContractType = contractType;
                            employeeDetail.DepartmentCode = workDayAdditionalSettings.Census_MapPayGroupAsDepCode == true ? emp.wdPayGroup : emp.WdLocationID;
                            employeeDetail.DepartmentName = emp.WdLocationName;
                            employeeDetail.Status = empStatus;
                            employeeDetail.HourlyRate = hourlyRate;
                            employeeDetail.SalaryAmount = Convert.ToDecimal(emp.WdTotalBasePayAnnualizedAmount); // per annum
                            employeeDetail.Comments = emp.WdPosition?.WdDescriptor;
                            employeeDetail.AlternateUserID = emp.WdPosition?.WdID?.Text;
                            employeeDetail.TerminationDate = termDate;
                            employeeDetail.State = emp.WdWorkState;


                            employeeDetail.State = workDayAdditionalSettings.Census_InformationToHide_csv != null && workDayAdditionalSettings.Census_InformationToHide_csv?.Split('|')?
                            .Any(val => val.Trim().Equals("State", StringComparison.InvariantCultureIgnoreCase)) == true ? null : emp.WdWorkState;

                            employeeDetail.HireDate = workDayAdditionalSettings.Census_InformationToHide_csv != null && workDayAdditionalSettings.Census_InformationToHide_csv?.Split('|')?
                            .Any(val => val.Trim().Equals("HireDate", StringComparison.InvariantCultureIgnoreCase)) == true ? DateTime.MinValue : Convert.ToDateTime(emp.WdHireDate);

                            employeeDetail.SSN = workDayAdditionalSettings.Census_InformationToHide_csv != null && workDayAdditionalSettings.Census_InformationToHide_csv?.Split('|')?
                            .Any(val => val.Trim().Equals("SSN", StringComparison.InvariantCultureIgnoreCase)) == true ? string.Empty : emp.wdLastfourSSN;
                            employeeDetail.EmailAddress = emp.wdEmail;

                            employeeDetail.DepartmentCode = workDayAdditionalSettings.Census_WorkdayHybridStates?.Split('|')
                                                                    .Any(val => val.Trim().Equals(employeeDetail.State, StringComparison.InvariantCultureIgnoreCase)) == true ?
                                                                        (workDayAdditionalSettings?.Census_WorkdayHybridDepartmentContateString ?? "H-") + employeeDetail.DepartmentCode : employeeDetail.DepartmentCode;


                            employeeDetail.TaxDeductionPercentage = workDayAdditionalSettings.DefaultTaxDeductionPercentageAll != null && workDayAdditionalSettings.DefaultTaxDeductionPercentageAll > 0
                                ? workDayAdditionalSettings.DefaultTaxDeductionPercentageAll
                                : null;

                            employeeDetailList.Add(employeeDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseEmployeeDetail.ResponseCode = System.Net.HttpStatusCode.InternalServerError;
                responseEmployeeDetail.ResponseMessage = ex.Message;
            }

            responseEmployeeDetail.Data = employeeDetailList;


            return responseEmployeeDetail;
        }

        public async Task<HRISGatewayInterface.Entities.Response<string>> GetCensusJSONString()
        {
            var responseEmployeeDetail = new HRISGatewayInterface.Entities.Response<string>();
            Entities.Response<string> censusResp = null;
            try
            {
                if ((string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusEndpoint) || string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusEndpoint)) && string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusURL)) throw new ArgumentNullException("Missing URL for census Syncing");
                if (!string.IsNullOrWhiteSpace(workDayAdditionalSettings.BaseURL) && !workDayAdditionalSettings.BaseURL.LastOrDefault().Equals('/')) workDayAdditionalSettings.BaseURL += "/";
                if (!string.IsNullOrWhiteSpace(workDayAdditionalSettings.CensusEndpoint) && !workDayAdditionalSettings.CensusEndpoint.FirstOrDefault().Equals('/')) workDayAdditionalSettings.CensusEndpoint = "/" + workDayAdditionalSettings.CensusEndpoint;

                var url = workDayAdditionalSettings.CensusURL ?? (workDayAdditionalSettings.BaseURL + workDayAdditionalSettings.Username.Split('@')[0] + workDayAdditionalSettings.CensusEndpoint);
                if (url.Contains("?") && url.Split('?')[1].Contains("format", StringComparison.InvariantCultureIgnoreCase))
                {
                    url = url.Split('?')[0] + "?format=JSON";
                }
                else
                {
                    url = url + "?format=JSON";
                }

                censusResp = await ExecuteRequest(url);
                responseEmployeeDetail.ResponseCode = censusResp.StatusCode;
                responseEmployeeDetail.ResponseMessage = censusResp.ResponseMessage;
                responseEmployeeDetail.Data = censusResp.Data;
            }
            catch (Exception ex)
            {
                responseEmployeeDetail.ResponseCode = System.Net.HttpStatusCode.InternalServerError;
                responseEmployeeDetail.ResponseMessage = ex.Message + "\n" + ex.StackTrace;
            }

            return responseEmployeeDetail;
        }

        #endregion

        #region Timesheets

        public async Task<HRISGatewayInterface.Entities.Response<IEnumerable<TimeCard>>> GetTimeAttendance(DateTime startDate, DateTime endDate)
        {
            var responseTimeCard = new HRISGatewayInterface.Entities.Response<IEnumerable<TimeCard>>();
            var timeCards = new List<TimeCard>();

            try
            {

                if (!string.IsNullOrWhiteSpace(workDayAdditionalSettings.BaseURL) && !workDayAdditionalSettings.BaseURL.LastOrDefault().Equals('/')) workDayAdditionalSettings.BaseURL += "/";
                if (!string.IsNullOrWhiteSpace(workDayAdditionalSettings.TAEndpoint) && !workDayAdditionalSettings.TAEndpoint.FirstOrDefault().Equals('/')) workDayAdditionalSettings.TAEndpoint = "/" + workDayAdditionalSettings.TAEndpoint;

                if (string.IsNullOrWhiteSpace(workDayAdditionalSettings.TAURL) && (string.IsNullOrWhiteSpace(workDayAdditionalSettings.BaseURL) || string.IsNullOrWhiteSpace(workDayAdditionalSettings.TAEndpoint)))
                    throw new ArgumentNullException("TAURL or BaseURL+TAEndpoint, cannot be null or empty!");

                var parameters = $"?End_Date={endDate.ToString("yyyy-MM-dd")}&Start_Date={startDate.ToString("yyyy-MM-dd")}";
                var url = (workDayAdditionalSettings.TAURL ?? workDayAdditionalSettings.BaseURL + workDayAdditionalSettings.Username.Split('@')[0] + workDayAdditionalSettings.TAEndpoint) + parameters;
                var response = new Entities.Response<string>();
                response = await ExecuteRequest(url);
                string taResponse = "";
                if (response != null && response.Data != null && !string.IsNullOrWhiteSpace(response.Data))
                {
                    taResponse = response.Data;
                }
                else
                {
                    responseTimeCard.ResponseMessage = response.ResponseMessage;
                    responseTimeCard.ResponseCode = HttpStatusCode.NoContent;
                }

                if (!string.IsNullOrWhiteSpace(taResponse))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(taResponse);
                    var jsonText = JsonConvert.SerializeXmlNode(doc);
                    //dynamic jsonObj = JsonConvert.DeserializeObject(jsonText);
                    var taData = JsonConvert.DeserializeObject<WorkdayTAData>(jsonText);

                    if (taData.WdReportData != null && taData.WdReportData.WdReportEntry.Count() > 0 && taData.WdReportData.WdReportEntry.Any())
                    {
                        foreach (var timecard in taData.WdReportData.WdReportEntry)
                        {
                            try
                            {
                                TimeCard tc = new TimeCard();

                                var employeeId =
                                            (from x in timecard.WdWorker.WdID
                                             where x.WdType == "Employee_ID"
                                             select x.Text).FirstOrDefault();

                                decimal hours = Convert.ToDecimal(timecard.WdHours);

                                var fullName = timecard.WdWorker.WdDescriptor.Split(" ");

                                tc.InternalEmployeeID = employeeId;
                                tc.FirstName = fullName[0];
                                tc.MiddleName = fullName.Length > 2 ? fullName[1] : null;
                                tc.LastName = fullName.Length > 1 ? fullName[fullName.Length - 1] : null;
                                tc.PayCode = timecard?.wdTimeEntryCode?.WdDescriptor ?? "REG";
                                tc.ShiftTimeInHours = hours;
                                tc.ShiftTimeInMinutes = (int)(hours * 60);
                                tc.ShiftStartTime = Convert.ToDateTime(timecard.WdDate).Date;
                                tc.ShiftEndTime = Convert.ToDateTime(timecard.WdDate).Date;

                                timeCards.Add(tc);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        if (timeCards != null && timeCards.Any())
                        {
                            var timeCardsList = new List<TimeCard>();
                            var DistinctUsersTimeCards = timeCards.GroupBy(x => new { x.InternalEmployeeID, x.ShiftStartTime });
                            if (DistinctUsersTimeCards != null && DistinctUsersTimeCards.Any())
                            {
                                foreach (var hours in DistinctUsersTimeCards)
                                {
                                    try
                                    {
                                        var firstRecord = hours.FirstOrDefault();
                                        timeCardsList.Add(new TimeCard
                                        {
                                            InternalEmployeeID = firstRecord.InternalEmployeeID,
                                            FirstName = firstRecord.FirstName,
                                            MiddleName = firstRecord.MiddleName,
                                            LastName = firstRecord.LastName,
                                            PayCode = firstRecord.PayCode,
                                            ShiftTimeInHours = hours.Sum(x => x.ShiftTimeInHours),
                                            ShiftTimeInMinutes = hours.Sum(x => x.ShiftTimeInMinutes),
                                            ShiftStartTime = firstRecord.ShiftStartTime,
                                            ShiftEndTime = firstRecord.ShiftEndTime
                                        });
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }

                                if (timeCardsList != null && timeCardsList.Any())
                                {
                                    timeCards = timeCardsList;
                                }

                            }
                        }

                        responseTimeCard.Data = timeCards;
                    }
                }
            }
            catch (Exception ex)
            {
                responseTimeCard.ResponseCode = System.Net.HttpStatusCode.InternalServerError;
                responseTimeCard.ResponseMessage = ex.Message;
            }

            return responseTimeCard;
        }

        #endregion

        private async Task<Entities.Response<string>> ExecuteRequest(string url)
        {
            var response = new Entities.Response<string>();

            try
            {
                var request = new RestRequest(url, Method.GET);
                request.Timeout = 360000;
                request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(workDayAdditionalSettings.Username.Split('@')[0] + ":" + workDayAdditionalSettings.Password))}");
                request.AddHeader("Content-Type", "application/xml");
                var apiresponse = await client.ExecuteAsync(request);
                response.StatusCode = apiresponse.StatusCode;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.Data = apiresponse.Content;
                }
                else
                {
                    //response.Data = apiresponse.ErrorMessage;
                    response.ResponseMessage = apiresponse.ErrorMessage;
                }
                #region Commented Code

                //var handler = new HttpClientHandler();
                //handler.UseCookies = false;


                //using (var httpClient = new HttpClient(handler))
                //{
                //    using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                //    {
                //        httpClient.Timeout = TimeSpan.FromSeconds(360);
                //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(workDayAdditionalSettings.Username + ":" + workDayAdditionalSettings.Password))}");

                //        var apiResponse =  httpClient.SendAsync(request).GetAwaiter().GetResult();
                //        if (response.StatusCode == HttpStatusCode.OK)
                //        {
                //            response.Data = apiResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult(); 
                //        }
                //        else
                //        {
                //            response.Data = apiResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult(); 
                //            response.ResponseMessage = apiResponse.Content.ReadAsStringAsync().Result;
                //        }
                //    }
                //} 
                #endregion

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Message :{0} ", e.Message);
                response.ResponseMessage = $"Exception Message: {e.Message}\nStack trace: {e.StackTrace}";
                return response;
            }
        }

        public async Task<WdGetPayrollSummaryResponse> GetPayrollSummary(DateTime startDate, DateTime endDate, int pageSize = 10, int PageNumber = 1, string employeeID = null)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (PageNumber < 1)
                PageNumber = 1;
            if (pageSize < 1)
                pageSize = 250;
            if (!string.IsNullOrWhiteSpace(employeeID))
                PageNumber = 1;

            var wdGetPayrollSummaryResponse = new WdGetPayrollSummaryResponse();



            //using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://services1.myworkday.com/ccx/service/rhahealthservices/Payroll/v36.2"))
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), workDayAdditionalSettings.PayrollSummaryUrl))
            {
                var employeeFilter = string.IsNullOrWhiteSpace(employeeID) ? string.Empty :
                    $" <wd:Employee_Reference>\n  <wd:ID wd:type=\"Employee_ID\">{employeeID.Trim()}</wd:ID>\n </wd:Employee_Reference>\n  ";


                var reqBody = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<env:Envelope\n    xmlns:env=\"http://schemas.xmlsoap.org/soap/envelope/\"\n    xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n    <env:Header>\n        <wsse:Security\n        env:mustUnderstand=\"1\"\n        xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\n            <wsse:UsernameToken>" +
                    $"\n<wsse:Username>{workDayAdditionalSettings.Username.Split('@')[0]}</wsse:Username>\n " +
                    $"<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{workDayAdditionalSettings.Password}</wsse:Password>\n" +
                    "</wsse:UsernameToken>\n        </wsse:Security>\n    </env:Header>\n    <env:Body>\n       <wd:Get_Payroll_Results_Request\n            xmlns:wd=\"urn:com.workday/bsvc\"\n >\n            <wd:Request_Criteria>\n" +
                    $" <wd:Start_Date>{startDate.ToString("yyyy-MM-dd")}</wd:Start_Date>\n" +
                    $" <wd:End_Date>{endDate.ToString("yyyy-MM-dd")}</wd:End_Date>\n" +
                    " \n<wd:Period_Selection_Date_Reference wd:Descriptor=\"Period_Date_Indicator\">\n " +
                    "<wd:ID wd:type=\"Period_Date_Indicator\">Based on Period Start Date</wd:ID>\n" +
                    " </wd:Period_Selection_Date_Reference> -->\n" +
                    employeeFilter +
                    " </wd:Request_Criteria>\n            <wd:Response_Filter>\n                " +
                    $"<wd:Page>{PageNumber}</wd:Page>\n   " +
                    $"<wd:Count>{pageSize}</wd:Count>\n " +
                    " </wd:Response_Filter>\n            <wd:Response_Group>\n<!-- Optional: -->\n<wd:Include_Name_Data>true</wd:Include_Name_Data>\n<!-- Optional: -->\n<wd:Include_National_ID_Data>false</wd:Include_National_ID_Data>\n<!-- Optional: -->\n<wd:Include_Related_Calculation_Result_Data>false</wd:Include_Related_Calculation_Result_Data>\n<!-- Optional: -->\n<wd:Include_Withholding_Order_Data>false</wd:Include_Withholding_Order_Data>\n<!-- Optional: -->\n<wd:Include_Payroll_Worktag_Data>false</wd:Include_Payroll_Worktag_Data>\n</wd:Response_Group>\n        </wd:Get_Payroll_Results_Request>\n    </env:Body>\n</env:Envelope>";

                request.Content = new StringContent(reqBody);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/xml");

                var response = await httpClient.SendAsync(request);


                wdGetPayrollSummaryResponse.StatusCode = response.StatusCode;

                var responseData = response.Content.ReadAsStringAsync().Result;
                wdGetPayrollSummaryResponse.ResponseMessage = response.IsSuccessStatusCode ? null : response.ReasonPhrase + responseData;
                if (!response.IsSuccessStatusCode)
                    return wdGetPayrollSummaryResponse;
                var doc = new XmlDocument();
                doc.LoadXml(responseData);
                dynamic dynamic = JsonConvert.DeserializeObject(JsonConvert.SerializeXmlNode(doc));

                var body = dynamic["env:Envelope"]["env:Body"]["wd:Get_Payroll_Results_Response"];

                wdGetPayrollSummaryResponse.TotalPages = (int)body["wd:Response_Results"]["wd:Total_Pages"];
                wdGetPayrollSummaryResponse.CurrPage = (int)body["wd:Response_Results"]["wd:Page"];
                wdGetPayrollSummaryResponse.PageSize = (int)body["wd:Response_Results"]["wd:Page_Results"];


                var payrollData = body["wd:Response_Data"]["wd:Payroll_Result"];

                if (wdGetPayrollSummaryResponse.EmployeePayrollSummaries == null)
                    wdGetPayrollSummaryResponse.EmployeePayrollSummaries = new List<WdEmployeePayrollSummary>();



                foreach (var empPayrollInfo in payrollData)
                {
                    try
                    {
                        wdGetPayrollSummaryResponse.EmployeePayrollSummaries.Add(new WdEmployeePayrollSummary()
                        {
                            EmployeeID = empPayrollInfo["wd:Employee_Reference"]["wd:ID"][1]["#text"],
                            GrossAmount = (double)empPayrollInfo["wd:Gross_Amount"],
                            NetAmount = (double)empPayrollInfo["wd:Net_Amount"]
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //return wdGetPayrollSummaryResponse;
            }


            return wdGetPayrollSummaryResponse;
        }
    }
}
