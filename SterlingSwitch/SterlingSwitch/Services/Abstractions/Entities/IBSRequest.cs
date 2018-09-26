namespace switch_mobile.Services.Abstractions.Entities
{
    public class IBSRequest
    {
       public string ReferenceID { get; set; }
        public int RequestType { get; set; }
        public int AppCode { get; set; }
        public string Title { get; set; }
        public string mobile { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string AddressHome { get; set; }
        public string bvn { get; set; }
        public string email { get; set; }
    }
}
