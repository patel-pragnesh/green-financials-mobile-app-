using System;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class SaveSwitchBeneficiary
    {
        public string switchuserid { get; set; }
        public string beneficiaryMobile { get; set; }
        public string beneficiaryNuban { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime dateModified { get; set; }
        public bool StatusFlag { get; set; }
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }

        public string BenName { get; set; }
        public string BenNickName { get; set; }

        public string Bank { get; set; }
        public string BankCode { get; set; }
        public string TransactionType { get; set; }


        public class GetSavedBeneficiaries
        {
            public string switchuserid { get; set; }
            public string beneficiaryMobile { get; set; }
            public string beneficiaryNuban { get; set; }
            public DateTime dateAdded { get; set; }
            public DateTime dateModified { get; set; }
            public bool StatusFlag { get; set; }
            public string Referenceid { get; set; }
            public int RequestType { get; set; }
            public string category { get; set; }
            public string Translocation { get; set; }
            public string TransactionType { get; set; }
            public string BenName { get; set; } = "NA";
            public string BenNickName { get; set; } = "NA";
            public bool IsVisible { get; set; }
            public string Bank { get; set; } = "NA";
            public string BankCode { get; set; } = "NA";
            public string display { get { return BenName + " " + beneficiaryNuban + " " + Bank; } }
            public string tabbedDisplay { get { return BenName + "|" + beneficiaryNuban + "|" + Bank + "|" + BankCode; } }
        }

        public class GetBeneRequest
        {
            public string switchuserid { get; set; }
            public string Referenceid { get; set; }
            public int RequestType { get; set; }
            public string Translocation { get; set; }
        }
    }
}
