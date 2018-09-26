using SterlingSwitch.Models;
using System;
using System.Xml.Serialization;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class FixedDepositModel : IBSRequest
    {       
        public string changeperiod { get; set; }
        public string payinacct { get; set; }
        public string payoutacct3 { get; set; }
        public string payoutacct2 { get; set; }
        public string payoutacct1 { get; set; }
        public string customerid { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string rate { get; set; }
        public string effectivedate { get; set; } 
    }
}
