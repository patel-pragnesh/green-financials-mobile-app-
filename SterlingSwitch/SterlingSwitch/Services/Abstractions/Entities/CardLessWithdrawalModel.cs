namespace SterlingSwitch.Services.Abstractions.Entities
{

    public class CardLessWithdrawalModel
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string appid { get; set; }
        public string ttid { get; set; }
        public double amount { get; set; }
        public string codeGenerationChannel { get; set; }
        public string accountNo { get; set; }
        public string transactionRef { get; set; }
        public string subscriber { get; set; }
        public string oneTimePin { get; set; }
    }

}
