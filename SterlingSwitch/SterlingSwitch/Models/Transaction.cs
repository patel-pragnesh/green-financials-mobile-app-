using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
       
    public class Transaction
    {
        public string AccountNo { get; set; }
        public string BalanceBF { get; set; }
        public string BalanceCF { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string Remarks { get; set; }
        public DateTime TraDate { get; set; }
        public string TxnRef { get; set; }
        public DateTime ValDate { get; set; }
    }

}
