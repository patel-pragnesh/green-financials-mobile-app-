using Android.Content;
using SterlingSwitch.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ScrollView), typeof(CustomScrollViewRenderer))]
namespace SterlingSwitch.Droid.Renderers
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        public CustomScrollViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;

            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;

            e.NewElement.PropertyChanged += OnElementPropertyChanged;



        }

        protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (ChildCount > 0)
                {
                    GetChildAt(0).HorizontalScrollBarEnabled = false;
                    GetChildAt(0).VerticalScrollBarEnabled = false;
                }
            }
            catch (System.Exception)
            {

            }

        }
    }
}