namespace switch_mobile.Services.Abstractions.Entities
{
    public class SterlingToWallet
    {
        public string Referenceid { get; set; }
        public string RequestType { get; set; }
        public string Translocation { get; set; }
        public string amt { get; set; }
        public string tellerid { get; set; }
        public string frmacct { get; set; }
        public string toacct { get; set; }
        public string paymentRef { get; set; }
        public string remarks { get; set; }
    }
}
