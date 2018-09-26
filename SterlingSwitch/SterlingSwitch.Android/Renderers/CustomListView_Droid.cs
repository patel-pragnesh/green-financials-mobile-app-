using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Java.Util;
using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListView_Droid))]
namespace SterlingSwitch.Droid.Renderers
{
    public class CustomListView_Droid:ListViewRenderer
    {

        public CustomListView_Droid(Context context):base(context)
        {
            
        }
        private CustomListView element;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.VerticalScrollBarEnabled = false;
            }
        }

    }
}