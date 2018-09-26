using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SterlingSwitch.Models;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class AirtimeRequest : IBSRequest
    {
        public string Mobile { get; set; }
        public string Beneficiary { get; set; }

        public string Amount { get; set; }
        public string NUBAN { get; set; }
        public string NetworkID { get; set; }
        public string Type { get; set; }

    }
}
