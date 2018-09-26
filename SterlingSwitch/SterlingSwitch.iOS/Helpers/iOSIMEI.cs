using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using Foundation;
using SterlingSwitch.Helper;
using SterlingSwitch.iOS.Helpers;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(iOSIMEI))]

namespace SterlingSwitch.iOS.Helpers
{
    public class iOSIMEI: IDevice
    {
        
      

        public string GetIdentifier()
        {
            return "";
        }
        public string GetDeviceModel()
        {
            return "";
        }

       
    }
}