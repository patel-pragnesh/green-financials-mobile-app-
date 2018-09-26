using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SterlingSwitch.Models
{
    public enum DocumentType
    {
        [Description("Photo ID")]
        InternationalPassport = 1,
        [Description("Photo ID")]
        DriversLicense ,
        [Description("Photo ID")]
        NationalIDCard,
        [Description("Address Verification")]
        UtilityBill,
        [Description("Address Verification")]
        RentReceipt,
        [Description("Address Verification")]
        TelephoneBill
    }

    
}
