using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class DebitAnyBankCardWithOTP
    {
        public string pin { get; set; }
        public string expiry_date { get; set; }
        public string cvv2 { get; set; }
        public string pan { get; set; }
        public string paymentId { get; set; }
        public string otp { get; set; }
        public string CreditAccount { get; set; }
        public string amount { get; set; }
    }

}
