namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class FundAccountResp
    {
        public string paymentId { get; set; }
        public string message { get; set; }
        public string responseCode { get; set; }
        public string amount { get; set; }
        public string transactionRef { get; set; }
    }
}
