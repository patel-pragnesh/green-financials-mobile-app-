using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BuyParthianBills
    {        
            public string Referenceid { get; set; }
            public int RequestType { get; set; }
            public string Translocation { get; set; }
            public string TransactionReference { get; set; }
            public string InstrumentsID { get; set; }
            public string PhoneNumber { get; set; }
            public string Amount { get; set; }
        public string Nuban { get; set; }

    }
}
