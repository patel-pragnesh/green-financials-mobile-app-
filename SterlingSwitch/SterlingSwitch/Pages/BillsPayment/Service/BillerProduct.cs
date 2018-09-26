using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Pages.BillsPayment.Service
{
    public class BillerProduct
    {
        public string ItemName { get; set; }
        public string PaymentCode { get; set; }
        public string Amount { get; set; }
        public string BillerID { get; set; }
    }
}
