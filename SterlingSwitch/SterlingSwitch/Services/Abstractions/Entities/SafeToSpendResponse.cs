using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class SafeToSpendResponse
    {
  
            public bool Status { get; set; }
            public string Message { get; set; }
            public DataBody Data { get; set; }
       

        public class DataBody
        {
            public string CurrentBalance { get; set; }
            public int Spent { get; set; }
            public int PendingSchedule { get; set; }
            public string MonthName { get; set; }
        }

    }
}
