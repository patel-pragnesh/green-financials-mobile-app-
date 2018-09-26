using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{

    public class SpectaEligibleAmountResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public int eligibleAmount { get; set; }
        public float monthlyRepayment { get; set; }
        public Fee[] fees { get; set; }
        public string repaymentDay { get; set; }
    }

    public class Fee
    {
        public string feeName { get; set; }
        public int percentage { get; set; }
        public int value { get; set; }
    }


}
