using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BuyDataRequestModel
    {
        public string Referenceid { get; set; }
        public string RequestType { get; set; }
        public string Translocation { get; set; }
        public string amt { get; set; }
        public string paymentcode { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string SubscriberInfo1 { get; set; }
        public string ActionType { get; set; }
        public string nuban { get; set; }
    }
}
