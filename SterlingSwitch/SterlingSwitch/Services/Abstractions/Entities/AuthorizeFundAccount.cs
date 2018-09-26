using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class AuthorizeFundAccount
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string RefID { get; set; }
        public string OTP { get; set; }
    }
}
