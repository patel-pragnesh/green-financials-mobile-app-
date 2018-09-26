using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class NetworkProviderModel
    {

        public class Rootobject
        {
            public Provider[] Providers { get; set; }
        }

        public class Provider
        {
            public string MobileNetwork { get; set; }
            public string BillID { get; set; }
            public string NetworkID { get; set; }
        }

    }
}
