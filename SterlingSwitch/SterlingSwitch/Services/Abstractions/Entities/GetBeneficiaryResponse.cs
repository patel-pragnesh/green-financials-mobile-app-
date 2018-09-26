using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
  

        public class GetBeneficiaryResponse
        {
            public beneficiaryResponse[] Data { get; set; }
        }

        public class beneficiaryResponse
        {
            public object switchuserid { get; set; }
            public string beneficiaryMobile { get; set; }
            public string beneficiaryNuban { get; set; }
            public DateTime dateAdded { get; set; }
            public DateTime dateModified { get; set; }
            public bool StatusFlag { get; set; }
            public object Referenceid { get; set; }
            public int RequestType { get; set; }
            public object Translocation { get; set; }
        }
}
