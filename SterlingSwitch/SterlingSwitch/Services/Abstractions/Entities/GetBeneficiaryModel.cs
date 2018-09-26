using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class GetBeneficiaryModel
    {

        public class Rootobject
        {
            public object message { get; set; }
            public object response { get; set; }
            public string responsedata { get; set; }
            public object data { get; set; }
        }

    }
}
