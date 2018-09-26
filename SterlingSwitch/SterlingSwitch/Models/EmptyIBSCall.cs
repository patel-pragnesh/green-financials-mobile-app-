using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SterlingSwitch.Models
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class EmptyIBSCall: IBSRequest
    {
    }
}
