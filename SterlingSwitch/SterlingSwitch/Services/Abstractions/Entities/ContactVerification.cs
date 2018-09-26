namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class ContactVerification
    {
        public string UserEmail { get; set; }
        public string Country { get; set; }
        public string ResidentialAddr1 { get; set; }
        public string ResidentialAddr2 { get; set; }
        public string StateAndProvince { get; set; }
        public string NameOfContact { get; set; }
        public string ContactResidentialAddress1 { get; set; }
        public string ContactResidentialAddress2 { get; set; }
    }
}
