namespace SterlingSwitch.Services.Abstractions.Entities
{
  public class CreateInstantAccount
    {      

        public string ReferenceID { get; set; }
        public string RequestType { get; set; }
        public string Title { get; set; }
        public string mobile { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string AddressHome { get; set; }
        public string bvn { get; set; }
        public string email { get; set; }
        public string Gender { get; set; }
		public string productCode { get; set; }
        public string currencyCode { get; set; }
    }
}
