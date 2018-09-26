using SterlingSwitch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace switch_mobile.Services.Abstractions.Entities
{
    [XmlRoot(ElementName = "IBSRequest")]
    public class W2W : IBSRequest
    {
	
		public string Translocation { get; set; }
		public string amt { get; set; }
		public string tellerid { get; set; }
		public string frmacct { get; set; }
		public string toacct { get; set; }
		public string exp_code { get; set; }
		public string paymentRef { get; set; }
		public string remarks { get; set; }
	}

    [XmlRoot(ElementName = "IBSRequest")]
    public class InterbankNameInquiry : IBSRequest
    {
		// request type 105
	
		public string ToAccount { get; set; }
		public string DestinationBankCode { get; set; }
	}


    [XmlRoot(ElementName = "IBSRequest")]
    public class IntraBankNameInquiry: IBSRequest
	{

		/*
		 * Request type 219
		 */

	
		public string NUBAN { get; set; }
	}
}
