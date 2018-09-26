using System;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class UserInformation
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int LoginFailedCount { get; set; }
        public string LoginIPAddress { get; set; }
        public string CustomerTimeZone { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public bool AccountLocked { get; set; }
        public bool BVN { get; set; }
        public string Gender { get; set; }
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string HomeAddress { get; set; }
    }
}
