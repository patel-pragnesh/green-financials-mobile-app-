using Android.Widget;
using Plugin.CurrentActivity;
using SterlingSwitch.Droid;
using SterlingSwitch.PlatformSpecs;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastMessage))]
namespace SterlingSwitch.Droid
{
    public class ToastMessage : IToastMessage
    {
        public void ShowToast(string message)
        {
            var currentActivity = CrossCurrentActivity.Current.Activity;
            Toast.MakeText(currentActivity, message, ToastLength.Short).Show();

        }
    }
}