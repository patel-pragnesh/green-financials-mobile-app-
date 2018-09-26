namespace switch_mobile.Services.Abstractions.Entities
{
    public class MVISACardModel
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string systemsTraceAuditNumber { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public string senderAccountNumber { get; set; }
        public string transactionCurrencyCode { get; set; }
        public string recipientName { get; set; }
        public string recipientPrimaryAccountNumber { get; set; }
        public string amount { get; set; }
        public string businessApplicationId { get; set; }
        public string transactionIdentifier { get; set; }
        public string merchantCategoryCode { get; set; }
        public string sourceOfFundsCode { get; set; }
        public string name { get; set; }
        public string terminalId { get; set; }
        public string idCode { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string country { get; set; }
        public string zipCode { get; set; }
        public string destinationAmount { get; set; }
    }
}
