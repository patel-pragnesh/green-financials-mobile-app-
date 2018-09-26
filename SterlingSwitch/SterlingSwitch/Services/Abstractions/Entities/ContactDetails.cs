using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{

    public class ContactDetails
    {
        public string message { get; set; }
        public string response { get; set; }
        public object responsedata { get; set; }
        public ContactDetailsData data { get; set; }
    }

    public class ContactDetailsData
    {
        public string UserEmail { get; set; }
        public string Country { get; set; }
        public string ResidentialAddr1 { get; set; }
        public string ResidentialAddr2 { get; set; }
        public string StateAndProvince { get; set; }
        public string NameOfContact { get; set; }
        public string ContactResidentialAddress1 { get; set; }
        public string ContactResidentialAddress2 { get; set; }
        public string status { get; set; }
    }

    public class ContactUserEmail
    {
        public string UserEmail { get; set; }
    }
}
