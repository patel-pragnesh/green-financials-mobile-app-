using System;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class OTPRequest
    {
        public string Referenceid { get; set; }
        public string RequestType { get; set; }
        public string Translocation { get; set; }
        public string nuban { get; set; }
    }

    public class ValOTPRequest
    {        
        public string Referenceid { get; set; }
        public string RequestType { get; set; }
        public string Translocation { get; set; }
        public string mobile { get; set; }
        public string otp { get; set; }
    }
}
