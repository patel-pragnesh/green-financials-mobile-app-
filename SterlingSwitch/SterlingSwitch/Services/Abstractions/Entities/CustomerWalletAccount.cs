namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class CustomerWalletAccount: BaseDataObject
    {
        public string Referenceid { get; set; } 
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string MOBILE { get; set; }
        public string GENDER { get; set; }
        public string BIRTHDATE { get; set; }
        public string EMAIL { get; set; }
        public string NATIONALITY { get; set; }
        public string TARGET { get; set; }
        public int SECTOR { get; set; }
        public string ADDR_LINE1 { get; set; }
        public string ADDR_LINE2 { get; set; }
        public string CUST_TYPE { get; set; }
        public string MARITAL_STATUS { get; set; }
        public string TITLE { get; set; }
        public string CUST_STATUS { get; set; }
        public string RESIDENCE { get; set; }
        public string CATEGORYCODE { get; set; }      

    }
}
