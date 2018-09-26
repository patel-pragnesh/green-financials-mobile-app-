using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    


        public class TransactionsStatement
        {
            public Ibsresponses IBSResponse { get; set; }
        }

        public class Ibsresponses
        {
            public string ReferenceID { get; set; }
            public string RequestType { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseText { get; set; }
            public Transactions Transactions { get; set; }
        }

        public class Transactions
        {
            public Transaction[] Transaction { get; set; }
        }

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
