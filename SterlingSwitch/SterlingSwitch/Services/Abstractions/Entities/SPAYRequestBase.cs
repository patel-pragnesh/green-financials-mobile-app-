using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
   public class SPAYRequestBase
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
    }
}
