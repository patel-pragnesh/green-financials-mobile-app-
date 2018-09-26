using System.Xml.Serialization;

namespace switch_mobile.Services.Abstractions.Entities
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class OtherBanks: SterlingSwitch.Models.IBSRequest
    {
        public string SessionID { get; set; }
 
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public double Amount { get; set; }
        public string DestinationBankCode { get; set; }
        public string NEResponse { get; set; }
        public string BenefiName { get; set; }
        public string PaymentReference { get; set; }
    }
}
