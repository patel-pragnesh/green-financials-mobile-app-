using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class AccountAdviceResponse
    {

       
            public bool Status { get; set; }
            public string Message { get; set; }
            public Datax Data { get; set; }
       

        public class Datax
        {
            public Safetospend[] SafeToSpend { get; set; }
            public Overview[] Overview { get; set; }
        }

        public class Safetospend
        {
            public string AccountNo { get; set; }
            public string Currency { get; set; }
            public string CurrentBalance { get; set; }
            public string Spent { get; set; }
            public string PendingSchedule { get; set; }
            public string MonthName { get; set; }
            public string SafeToSpendAmount { get; set; }
            public double percentageToSpend { get; set; }
        }

        public class Overview
        {
            public string AccountNo { get; set; }
            public string Currency { get; set; }
            public string TotalExpense { get; set; }
            public string TotalIncome { get; set; }
            public float TotalSavings { get; set; }
            public string MonthName { get; set; }
        }

    }
}
