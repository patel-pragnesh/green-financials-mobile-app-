using System.Xml.Serialization;

namespace switch_mobile.Services.Abstractions.Entities
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class TransferSterlingToSterling: SterlingSwitch.Models.IBSRequest
    {
   
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string Amount { get; set; }
        public string PaymentReference { get; set; }
    }
}
