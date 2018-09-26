using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
  public  class WalletTransacttion
    {
        public string TRA_DATE { set; get; }
        public string currencycode { set; get; }
        public string amt { set; get; }
        public string AmountFormatted { set; get; }
        public string deb_cre_indaa1 { set; get; }
        public string val_date { set; get; }
        public string remarks { set; get; }
        public string Balance { set; get; }
        public string TransactionType { get; set; }
    }
}
