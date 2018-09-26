using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class GetInvestments
    {
        public string customerid { get; set; } 
        public string amount { get; set; }
        public string response { get; set; }
        public string responseCode { get; set; }
        public string message { get; set; }
        public string Ref { get; set; }
    }
}
