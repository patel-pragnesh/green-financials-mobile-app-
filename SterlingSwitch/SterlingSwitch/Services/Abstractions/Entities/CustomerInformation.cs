using Newtonsoft.Json;
using System;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    [JsonObject(Title = "CustomerInformation")]
    
    public class CustomerInformation : BaseDataObject
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string BVN { get; set; }
        public string Title { get; set; }
        public string HomeAddress { get; set; }
		public string ReferralCode { get; set; }
		public string RefferedBy { get; set; }
        public string Pin { get; set; }
        public string ConfirmPin { get; set; }
	}

    public class UserAlreadyExist
    {
        public string UserEmail { get; set; }
        public string Phone { get; set; }
    }

    public class ProfileUserDevice
    {
        public string email { get; set; }
        public string imei { get; set; }
    }

    public class GetProfileUserDevice
    {
        public string imei { get; set; }
    }

    public class DeviceInf
    {
        public string email { get; set; }
    }

}
