using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class CustomerInfoFromT24
    {


        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AltAccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BVN { get; set; }
        public string CustomerId { get; set; }
        public object AltNumber { get; set; }
        public float AvailBalance { get; set; }
        public float CurrentBalance { get; set; }
        public float BalanceLimit { get; set; }
        public string BraCode { get; set; }
        public string BranchName { get; set; }
        public string CurCode { get; set; }
        public string CurrencyName { get; set; }
        public string LedCode { get; set; }
        public string LedgerName { get; set; }
        public string SubAcctCode { get; set; }
        public string CustomerClass { get; set; }
        public string RestInd { get; set; }
        public string StaCode { get; set; }
        public string MobNum { get; set; }
        public string Email { get; set; }
        public object CustomerType { get; set; }
        public DateTime LastTransactDate { get; set; }
        public string Officer { get; set; }
        public string NAME_LINE1 { get; set; }
        public string NAME_LINE2 { get; set; }
        public DateTime DateOpened { get; set; }
        public float AvailBalanceHidden { get; set; }
        public string AccountTitle { get; set; }
        public string CourtesyTitle { get; set; }
        public string AccountGroup { get; set; }
        public string CustomerStatusCode { get; set; }
        public string responseCode { get; set; }      
        
            public float AccountBalance { get; set; }
            public string customerid { get; set; }
            public string firstname { get; set; }
            public string nuban { get; set; }
            public string bvn { get; set; }
            public string branchcode { get; set; }
            public string currencycode { get; set; }
            public string altcurrencycode { get; set; }
            public int restind { get; set; }
            public string stacode { get; set; }
            public object categorycode { get; set; }
            public float availablebalance { get; set; }
            public string lastname { get; set; }
            public string fullname { get; set; }
            public string mobile { get; set; }
            public string mobile2 { get; set; }
            public string phone { get; set; }
            public string phone2 { get; set; }
            public object gender { get; set; }
            public string email { get; set; }
            public DateTime dateopened { get; set; }
            public string ADDR_LINE1 { get; set; }
            public string ADDR_LINE2 { get; set; }
            public string status { get; set; }
            public int AccountStatus { get; set; }
       


    }
}
