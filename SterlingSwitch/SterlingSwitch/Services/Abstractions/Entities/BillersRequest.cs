namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BillersRequest
    {
        public string Referenceid { get; set; }
        public string RequestType { get; set; }
        public string Translocation { get; set; }
        public decimal amt { get; set; }
        public string paymentcode { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string SubscriberInfo1 { get; set; }
        public string ActionType { get; set; }
        public string nuban { get; set; }
    }
}
