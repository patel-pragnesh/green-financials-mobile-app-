using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
   
        public class WalletDetails
        {
        public WalletDetails()
        {
            data = new WData();
        }
            public string message { get; set; }
            public string response { get; set; }
            public object responsedata { get; set; }
            public WData data { get; set; }
        }

        public class WData
        {
            public string AccountName { get; set; }
            public string AccountNumber { get; set; }
            public double AccountBalance { get; set; }
            public string customerid { get; set; }
            public string firstname { get; set; }
            public string nuban { get; set; }
            public string branchcode { get; set; }
            public string currencycode { get; set; }
            public int restind { get; set; }
            public string stacode { get; set; }
            public string categorycode { get; set; }
            public double availablebalance { get; set; }
            public string lastname { get; set; }
            public string fullname { get; set; }
            public string mobile { get; set; }
            public string mobile2 { get; set; }
            public string phone { get; set; }
            public string phone2 { get; set; }
            public string gender { get; set; }
            public string email { get; set; }
            public DateTime dateopened { get; set; }
            public string ADDR_LINE1 { get; set; }
            public string ADDR_LINE2 { get; set; }
            public string status { get; set; }
            public int AccountStatus { get; set; }
        }

    
}
