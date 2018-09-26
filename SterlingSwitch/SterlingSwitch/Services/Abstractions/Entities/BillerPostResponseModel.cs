using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BillerPostResponseModel
    {

        
            public Biller[] Billers { get; set; }


        public class Biller
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ShortName { get; set; }
            public string Narration { get; set; }
            public string Surcharge { get; set; }
            public string CustomerFieldLabel { get; set; }
        }

    }
}
