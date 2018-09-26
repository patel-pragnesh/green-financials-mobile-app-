using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class TransferModel: SPAYRequestBase
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string Amount { get; set; }
        public string PaymentReference { get; set; }

        public string SessionID { get; set; }
        public int ReferenceID { get; set; }

        public string FromBank { get; set; }
        public string ToBank { get; set; }
        public string displayedAccount { get; set; }

        public string DestinationBankCode { get; set; }
        public string NEResponse { get; set; }
        public string BeneficiaryName { get; set; }
        public string TransactionType { get; set; }
        public string SelectedBeneficiary { get; set; }
    }
}
