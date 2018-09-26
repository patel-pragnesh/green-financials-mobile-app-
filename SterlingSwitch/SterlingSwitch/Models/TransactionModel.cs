using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SterlingSwitch.Models
{
    [XmlRoot(ElementName = "IBSRequest")]
   public class TransactionModel:IBSRequest
    {
        public string customerAccountNos { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
