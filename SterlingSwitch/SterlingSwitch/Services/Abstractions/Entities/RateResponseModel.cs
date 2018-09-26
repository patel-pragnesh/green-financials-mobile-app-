namespace switch_mobile.Services.Abstractions.Entities
{
    public class RateResponseModel
    {

        public string message { get; set; }
        public string response { get; set; }
        public object responsedata { get; set; }
        public Data data { get; set; }


        public class Data
        {
            public string status { get; set; }
            public object RequestId { get; set; }
            public object TransactionReference { get; set; }
            public object TransferType { get; set; }
            public object SystemTraceAuditNumber { get; set; }
            public object NetworkReferenceNumber { get; set; }
            public object SettlementDate { get; set; }
            public string ResponseCode { get; set; }
            public object Response_Description { get; set; }
            public object SubmitDateTime { get; set; }
        }


    }
}