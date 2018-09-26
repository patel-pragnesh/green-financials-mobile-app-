using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class GetRateModel : SPAYRequestBase
    {
            public string SourceAmt { get; set; }
            public string SourceCurrency { get; set; }
            public string DestinationCurrency { get; set; }
            public string TransactionType { get; set; }
    }
}
