namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class AirtimeRecharge
    {
        
         public string ReferenceID { get; set; }
        public int RequestType { get; set; }
        public string Mobile { get; set; }
        public string Beneficiary { get; set; }
        public string NUBAN { get; set; }
        public decimal Amount { get; set; }
        public string NetworkID { get; set; }
        public string Type { get; set; }
    }
}
