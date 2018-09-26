namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class FundWalletAccount
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string TransactionReference { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string recipient_bank { get; set; }
        public string recipient_account_number { get; set; }
        public int amount { get; set; }
        public string narration { get; set; }
        public string card_no { get; set; }
        public string cvv { get; set; }
        public string pin { get; set; }
        public string expiry_year { get; set; }
        public string charge_auth { get; set; }
        public string expiry_month { get; set; }
        public string fee { get; set; }
        public string medium { get; set; }
        public string redirecturl { get; set; }
        public string CreditAccount { get; set; }
    }
}
