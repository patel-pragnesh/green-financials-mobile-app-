using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class CustomerInfo
    {
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
        
    }
}
