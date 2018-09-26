using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class CrossCurrencytransfer
    {

        public string fromCurrency { get; set; }
        public string fromAccountNo { get; set; }
        public string fromAmount { get; set; }
        public string Remark { get; set; }
        public string ToCurrency { get; set; }
        public string ToAccount { get; set; }
        public string ToAmount { get; set; }


    }
}