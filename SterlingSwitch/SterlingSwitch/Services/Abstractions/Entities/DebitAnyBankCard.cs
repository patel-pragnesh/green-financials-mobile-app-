using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class DebitAnyBankCard
    {
        public string customerId { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string pin { get; set; }
        public string expiry_date { get; set; }
        public string cvv { get; set; }
        public string pan { get; set; }
        public string Referenceid { get; set; }
        public string RequestType { get; set; }
        public string Translocation { get; set; }
        public string CreditAccount { get; set; }
    }

    public class DebitAnyBankCardVISA
    {
        public string paymentId { get; set; }
        public string eciFlag { get; set; }
        public string transactionId { get; set; }
        public string pin { get; set; }
        public string expiry_date { get; set; }
        public string cvv { get; set; }
        public string pan { get; set; }
        public string CreditAccount { get; set; }
        public string amount { get; set; }
    }
}
