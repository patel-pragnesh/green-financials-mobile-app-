using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class ParthianInnerArrayOfBillDetails
    {
        public int TradeId { get; set; }
        public int InstrumentId { get; set; }
        public int TradeType { get; set; }
        public DateTime TradeDate { get; set; }
        public double InterestRate { get; set; }
        public DateTime MaturityDate { get; set; }
        public string MaturityDateFormatted { get; set; }
        public int TradeStatus { get; set; }
        public double AmountInvested { get; set; }
        public double CurrentBalance { get; set; }
        public double AmountWithdrawn { get; set; }
        public DateTime LastUpdated { get; set; }
        public double? SavedInterest { get; set; }
        public double ValueAtMaturity { get; set; }
    }
}
