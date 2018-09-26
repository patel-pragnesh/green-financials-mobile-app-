using Android.OS;
using SterlingSwitch.Droid.Helpers;
using SterlingSwitch.Helper;
using Xamarin.Forms;
using static Android.Provider.Settings;

[assembly: Dependency(typeof(AndroidIMEI))]

namespace SterlingSwitch.Droid.Helpers
{
    public class AndroidIMEI: IDevice
    {
        public string GetIdentifier()
        {         
            string _androidId = Secure.GetString(Forms.Context.ContentResolver, Secure.AndroidId);
            return _androidId.ToUpper();
        }

        public string GetDeviceModel()
        {
            string model = Build.Model;
            string manufacturer = Build.Manufacturer;
            string brand = Build.Brand;
            string message = model;
            return message;
        }
    }
}