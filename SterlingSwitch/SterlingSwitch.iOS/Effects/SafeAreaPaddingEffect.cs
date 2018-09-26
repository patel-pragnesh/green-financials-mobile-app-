using System;
using SterlingSwitch.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Sterling")]
[assembly: ExportEffect(typeof(SafeAreaPaddingEffect), nameof(SafeAreaPaddingEffect))]
namespace SterlingSwitch.iOS.Effects
{
    public class SafeAreaPaddingEffect: PlatformEffect
    {
        Thickness _padding;
        public SafeAreaPaddingEffect()
        {
        }

        protected override void OnAttached()
        {
            if (Element is Templates.SwitchMasterPage element)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    _padding = element.Padding;
                    var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets; // Can't use KeyWindow this early
                    if (insets.Top > 0) // We have a notch
                    {
                        if(element.Content is Xamarin.Forms.ContentView contentView)
                        {
                            if (contentView == null) return;
                            contentView.Padding = new Thickness(0, insets.Top - 10, 0, 0);
                            element.BackgroundColor = element.TopNavBarBackgroundColor;

                            // Set the contentView Content background color to white
                          //  var contentView_content = contentView.Content;
                            //contentView_content.BackgroundColor = Color.White;
                          

                        }
                       // element.Padding = new Thickness(_padding.Left + insets.Left, _padding.Top + insets.Top, _padding.Right + insets.Right, _padding.Bottom);
                        return;
                    }
                }
                // Uses a default Padding of 20. Could use an property to modify if you wanted.
               // element.Padding = new Thickness(_padding.Left, _padding.Top + 20, _padding.Right, _padding.Bottom);
            }
        }

        protected override void OnDetached()
        {
            if (Element is Layout element)
            {
                element.Padding = _padding;
            }
        }
    }
}
