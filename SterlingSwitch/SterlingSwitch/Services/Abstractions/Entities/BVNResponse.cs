namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BVNResponse
        {
            public string message { get; set; }
            public string response { get; set; }
            public BVNResponseData data { get; set; }
        }

        public class BVNResponseData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string PhoneNumber { get; set; }
            public string DateOfBirth { get; set; }
            public string RegistrationDate { get; set; }
            public string EnrollmentBankCode { get; set; }
            public string EnrollmentBranch { get; set; }
            public string status { get; set; }
        } 
}
