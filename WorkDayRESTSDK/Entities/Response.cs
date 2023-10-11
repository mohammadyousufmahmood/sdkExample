using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WorkDayRESTSDK.Entities
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }
          
        public bool IsError { get; }
    }
    
}
