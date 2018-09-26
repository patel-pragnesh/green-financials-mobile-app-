using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BillerItemResponse
    {  
            public Item[] Items { get; set; }
       
        public class Item
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ConsumerIdField { get; set; }
            public string Amount { get; set; }
            public string ConvertedAmount { get; set; }
            public string IsAmountFixed { get; set; }
            public string PaymentCode { get; set; }
        }

    }
}
