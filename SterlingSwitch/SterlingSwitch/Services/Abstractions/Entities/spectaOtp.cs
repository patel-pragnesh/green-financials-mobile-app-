using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class spectaOtp
    {

        public bool isOtpSent { get; set; }
        public string otp { get; set; }
        public string phoneNumberSentTo { get; set; }
        public string emailSentTo { get; set; }

    }
}
