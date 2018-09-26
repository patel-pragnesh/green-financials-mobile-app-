namespace SterlingSwitch.Services.Abstractions.Entities
{

    public class CreateWalletResponse
    {
        public string message { get; set; }
        public string response { get; set; }
        public ResponseData data { get; set; }
    }

    public class ResponseData
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; }
        public string status { get; set; }
    }
}
