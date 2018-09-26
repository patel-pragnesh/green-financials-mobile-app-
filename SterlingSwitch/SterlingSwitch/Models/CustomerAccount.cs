using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
    public class CustomerAccount
    {
        public string nuban { get; set; }
        public string currency { get; set; }
        public string currencyCode { get; set; }
        public string balance { get; set; }
        public string AccountNumberWithBalance { get; set; }
        public string CustomerId { get; set; }
        public string AccountName { get; set; }
        public object AccountGroup { get; set; }
        public string accountType { get; set; }
        public string BVN { get; set; }
        public string AccountBalanceAccountType
        {
            get
            {
                var convertedBalance = string.Format("{0:N1}", Convert.ToDecimal(balance));
                var FormattedAccountInfo = $"{nuban} | {currencyCode}{convertedBalance} | {accountType}";
                return FormattedAccountInfo;
              
            }
            set { AccountBalanceAccountType = value; }
        }
    }

   

}
