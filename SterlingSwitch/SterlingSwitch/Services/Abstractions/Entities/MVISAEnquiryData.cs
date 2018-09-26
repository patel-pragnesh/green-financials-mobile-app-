using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class MVISAEnquiryData
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string acquirerCountryCode { get; set; }
        public string acquiringBin { get; set; }
        public string primaryAccountNumber { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public string systemsTraceAuditNumber { get; set; }
    }
}
