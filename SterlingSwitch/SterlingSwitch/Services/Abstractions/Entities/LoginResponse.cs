using System;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class LoginResponse
    {
        public bool Status { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int LoginFailedCount { get; set; }
        public string BVN { get; set; }
        public string Gender { get; set; }
        public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int title { get; set; }
        public string WalletAcct { get; set; }
        public string customerID { get; set; }
        public string Parthian { get; set; }
        public string ProfilePix { get; set; }
        public bool IsSecurity { get; set; }
        public bool IsTPIN { get; set; }
        public string HomeAddress { get; set; }
        public string CurCode { get; set; }
        public string LedgerCode { get; set; }
        public string NUBAN { get; set; }
        public int IsVirtualCardActive { get; set; }
        public bool IsVirtualCard { get; set; }
        public string VirtualCard { get; set; }
        public string ReferralCode { get; set; }
        public string RefferedBy { get; set; }
        public bool IsBVNUploaded { get; set; }
        public bool IsUtilityBillUploaded { get; set; }
        public bool IsReferreeUploaded { get; set; }
        public bool IsPictureUploaded { get; set; }
        public DateTime DatePictureUploaded { get; set; }
        public DateTime DateBVNUploaded { get; set; }
        public DateTime DateUtilityUploaded { get; set; }
        public DateTime DateRefereeUploaded { get; set; }
        public bool IsAccountLock { get; set; }
    }
}

