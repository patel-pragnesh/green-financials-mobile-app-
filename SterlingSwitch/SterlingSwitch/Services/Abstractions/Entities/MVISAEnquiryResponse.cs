using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class MVISAEnquiryResponse
    {
        public string message { get; set; }
        public string response { get; set; }
        public string responsedata { get; set; }
        public object data { get; set; }
    }

    public class MVISAEnquiryResponseData
    {
        public string cardTypeCode { get; set; }
        public string billingCurrencyCode { get; set; }
        public int billingCurrencyMinorDigits { get; set; }
        public string issuerName { get; set; }
        public string cardIssuerCountryCode { get; set; }
        public string fastFundsIndicator { get; set; }
        public string pushFundsBlockIndicator { get; set; }
        public string onlineGambingBlockIndicator { get; set; }
        public string geoRestrictionInd { get; set; }
    }


    public class MVisaWatchListReq
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string city { get; set; }
        public string cardIssuerCountryCode { get; set; }
        public string referenceNumber { get; set; }
        public string name { get; set; }
    }

    public class WListData
    {
        public string referenceNumber { get; set; }
        public string ofacScore { get; set; }
        public string status { get; set; }
    }


    public class MVISAExchangeRate
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string destinationCurrencyCode { get; set; }
        public string markUpRate { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public string sourceAmount { get; set; }
        public string sourceCurrencyCode { get; set; }
        public string systemsTraceAuditNumber { get; set; }
    }


    public class ExchangeRateResponse
    {
        public string conversionRate { get; set; }
        public string destinationAmount { get; set; }
        public string markUpRateApplied { get; set; }
        public string originalDestnAmtBeforeMarkUp { get; set; }
    }
}
