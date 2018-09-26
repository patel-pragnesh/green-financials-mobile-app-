using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BackButton : StackLayout
	{
		public BackButton ()
		{
			InitializeComponent ();

            var tgr = new TapGestureRecognizer();
            tgr.Tapped += Tgr_Tapped;

            this.GestureRecognizers.Add(tgr);
        }

        public event EventHandler Clicked;

        public ButtonType ButtonType
        {
            set
            {
                var type = value;

                if (BackIcon != null)
                {
                    switch (type)
                    {
                        case ButtonType.Black:
                            BackIcon.Source = ImageSource.FromFile("BlackBackArrow");
                            break;
                        case ButtonType.White:
                            BackIcon.Source = ImageSource.FromFile("BackIcon");
                            break;
                    }
                }
            }
        }

        private void Tgr_Tapped(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);

        }
    }

    public enum ButtonType
    {
        White,
        Black
    }
}