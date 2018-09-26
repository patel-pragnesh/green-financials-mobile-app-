namespace SterlingSwitch.Pages.BillsPayment.Service
{
    public class BillerPaymentResponse
    {
        public string message { get; set; }
        public string response { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string billerResponse { get; set; }
        public string status { get; set; }
    }
}
