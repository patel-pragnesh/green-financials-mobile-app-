using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    
    public  class ResetPinRequest
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string UserID { get; set; }
        public string TPIN { get; set; }
    }
}
