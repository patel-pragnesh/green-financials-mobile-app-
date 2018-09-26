using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SterlingSwitch.Models
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class CardModel : IBSRequest
    {
        public string CustomerID { get; set; }
    }

    public class CardObject
    {
        public Ibsresponse IBSResponse { get; set; }
    }

    public class Ibsresponse
    {
        public string ReferenceID { get; set; }
        public string RequestType { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
    }
}
