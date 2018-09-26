
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CarouselView.FormsPlugin.Android;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Plugin.CurrentActivity;
using System;
using SterlingSwitch.Services;
using Xamarin.Forms;
using Android.Views;
using ImageCircle.Forms.Plugin.Droid;
using DLToolkit.Forms.Controls;

namespace SterlingSwitch.Droid
{
    [Activity(Label = "Switch", Icon = "@drawable/app_icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentUnhandledExceptionRaiser;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
          //  CrossCurrentActivity.Current.Init(ApplicationContext);
            ImageCircleRenderer.Init();
            CarouselViewRenderer.Init();
            CrossCurrentActivity.Current.Init(this, bundle);    // initialize plugin for camera
            //GlobalStaticFields.ConfigureAppCenter();    // configure appcenter
            GlobalStaticFields.ConfigureAppCenter();
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            Push.SetSenderId("713211319792");
            RegisterCenter();            
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            FormsControls.Droid.Main.Init(this);
            FlowListView.Init();
            LoadApplication(new App());

            HideStatusBar();
        }

        private void RegisterCenter()
        {
            Microsoft.AppCenter.AppCenter.Start("ios=c6a76a6d-f8f1-4555-85d1-62a545b651bc;android=026e3b37-e951-4347-8196-ba957754bd5b;uwp=44c96eca-062b-47a3-9626-b4e8524561e5", typeof(Analytics), typeof(Crashes), typeof(Push));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);            
        }

        public async override void OnBackPressed()
        {
            bool? result = await App.CallHardwareBackPressed();
            if (result == true)
                base.OnBackPressed();
            else if (result == null)
                Finish();
        }

        public void HideStatusBar()
        {
            var activity = (Activity)Forms.Context;
            var window = activity.Window;
            var attrs = window.Attributes;
            attrs.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
            window.Attributes = attrs;

            window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
            window.AddFlags(WindowManagerFlags.Fullscreen);

            var decorView = window.DecorView;

            var uiOptions =
                (int)Android.Views.SystemUiFlags.LayoutStable |
                (int)Android.Views.SystemUiFlags.LayoutHideNavigation |
                (int)Android.Views.SystemUiFlags.LayoutFullscreen |
                (int)Android.Views.SystemUiFlags.HideNavigation |
                (int)Android.Views.SystemUiFlags.Fullscreen |
                (int)Android.Views.SystemUiFlags.Immersive;

            decorView.SystemUiVisibility = (Android.Views.StatusBarVisibility)uiOptions;

            window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
        }

        private void AndroidEnvironmentUnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }        
    }
}

