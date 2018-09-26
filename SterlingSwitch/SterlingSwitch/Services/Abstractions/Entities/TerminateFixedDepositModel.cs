using SterlingSwitch.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class TerminateFixedDepositModel : IBSRequest
    {
        public string arrangementid { get; set; }
    }
}
