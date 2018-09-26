using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{

    public class WalletTransactionSummary
    {
        public WalletTranasctionLog[] TranasctionLists { get; set; }
    }

    public class WalletTranasctionLog
    {
        public string TRA_DATE { get; set; }
        public string TRA_DATE_Formatted { get; set; }
        public string currencycode { get; set; }
        public string amt { get; set; }
        public string deb_cre_indaa1 { get; set; }
        public string val_date { get; set; }
        public string remarks { get; set; }
        public string Balance { get; set; }
        public string AmountFormatted { set; get; }
        public string TransactionType { set; get; }
        public string BadgeText { set; get; }
    }


}
