using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{

    public class AllCustomerInfo
    {
        public int UserId { get; set; }

        public string UserEmail { get; set; }

        public string Password { get; set; }

        public int? Role { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public int? LoginFailedCount { get; set; }

        public string LoginIPAddress { get; set; }

        public string CustomerTimeZone { get; set; }

        public DateTime? LastAccessedDate { get; set; }

        public bool? AccountLocked { get; set; }

        public string BVN { get; set; }

        public string Gender { get; set; }

        public DateTime? DateCreated { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public int? title { get; set; }

        public string AccountType { get; set; }

        public string NUBAN { get; set; }

        public string BranchCode { get; set; }

        public string CurCode { get; set; }

        public string LedgerCode { get; set; }

        public int? ProfileType { get; set; }

        public string HomeAddress { get; set; }

        public string CUS_NUM { get; set; }

        public string WalletAcct { get; set; }

        public string TPIN { get; set; }

        public string Parthian { get; set; }




    }

}
