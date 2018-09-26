using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
   public class MobileDevice
    {
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public string IMEI { get; set; }
        public string OS { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnabled { get; set; }
    }
}
