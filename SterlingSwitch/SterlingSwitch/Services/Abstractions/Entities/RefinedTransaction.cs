using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class RefinedTransactions
    {
        public string TransactionDate { get; set; }
        public bool IsCredit { get; set; }
        public string AccountNo { get; set; }
        public int ID { get; set; }
        public int UserId { get; set; }
        public object Username { get; set; }
        public string InitiatorFirstName { get; set; }
        public string InitiatorLastName { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryBVN { get; set; }
        public string Amount { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string PaymentReference { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string RequestType { get; set; }
        public string ReferenceID { get; set; }
        public bool IsBeneficiary { get; set; }
        public Color AmountColor { get; set; }
        // public string CurrencyCode { set; get; }
    }
}
