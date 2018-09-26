using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class AllAccountOfCustomer
    {
        public string nuban { get; set; }
        public string currency { get; set; }
        public string balance { get; set; }
        public string CustomerId { get; set; }
        public string currencyCode { get; set; }
        public string accountType { get; set; }
        public string BVN { get; set; }
        public string concatenateCurrencyCodeAndNuban { get; set; }
        public string accountLimit { get; set; }
        public double opacity { get; set; } = 1;
        public string AccountBalanceAccountType
        {
            get
            {
                var convertedBalance = $"{Convert.ToDecimal(balance):N1}";
                var formattedAccountInfo = $"{nuban} | {currencyCode}{convertedBalance} | {accountType}";
                return formattedAccountInfo;
                //return nuban + " " + currencyCode + convertedBalance + " " + accountType;
            }
            set { AccountBalanceAccountType = value; }
        }
    }
        
    } 

