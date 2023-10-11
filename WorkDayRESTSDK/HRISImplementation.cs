using HRISGatewayInterface;
using HRISGatewayInterface.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WorkDayRESTSDK.Entities;

namespace WorkDayRESTSDK
{
    public class HRISImplementation : ITAProvider, ICensusProvider
    {
        WorkDayRESTClient client = null;
        public void Init(ProviderSetting providerSetting, EmployerSetting employerSetting)
        {
            var additionalSettings = JsonConvert.DeserializeObject<WorkDayAdditionalSettings>(employerSetting.AdditionalSettings);
            client = new WorkDayRESTClient(additionalSettings);
        }

        public HRISGatewayInterface.Entities.Response<IEnumerable<EmployeeDetail>> GetCensus(CensusRequest request)
        {
            return client.GetCensus().GetAwaiter().GetResult();
        }

        public HRISGatewayInterface.Entities.Response<IEnumerable<TimeCard>> GetTimeCards(TARequest request)
        {
            return client.GetTimeAttendance(request.FromDate, request.ToDate).GetAwaiter().GetResult();
        }

        public WdGetPayrollSummaryResponse GetPayrollSummary(DateTime startDate, DateTime endDate, int pageSize = 10, int PageNumber = 1, string employeeID = null)
        {
            return client.GetPayrollSummary(startDate: startDate,endDate: endDate,pageSize,PageNumber,employeeID).GetAwaiter().GetResult();
        }

        #region Unimplemented Methods

        public HRISGatewayInterface.Entities.Response<IEnumerable<EmployeeInfoAndTimeCard>> GetEmployeeInfoAndTimeCards(TARequest request)
        {
            throw new NotImplementedException();
        }

        public TokenResponse GetAccessToken(TokenRequest request)
        {
            throw new NotImplementedException();
        }

        public TokenResponse RefreshAccesstoken(TokenRequest request)
        {
            throw new NotImplementedException();
        } 
        #endregion

    }
}
