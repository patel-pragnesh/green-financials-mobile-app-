using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
   
        public class SuccessfulParthianInvestment
        {
            public Portfoliobalance portfolioBalance { get; set; }
            public string RespCode { get; set; }
            public string RespDesc { get; set; }
    }

        public class Portfoliobalance
        {
            public float AmountInvested { get; set; }
            public float TotalInvestment { get; set; }
            public float InvestmentValue { get; set; }
            public string PortfolioName { get; set; }
            public int PortfolioId { get; set; }
            public int InvestmentsCount { get; set; }
            public string responseCode { get; set; }
            public string responseDesc { get; set; }

    }


}
