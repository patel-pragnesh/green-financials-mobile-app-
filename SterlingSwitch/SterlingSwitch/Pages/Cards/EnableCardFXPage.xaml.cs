using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EnableCardFXPage : ContentPage
	{
		public EnableCardFXPage ()
		{
			InitializeComponent ();
		}

        public event EventHandler<bool> CardEnabled;

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            CardEnabled?.Invoke(this, true);
            Navigation.PopModalAsync(true);
        }

        private void DeclineBtn_Clicked(object sender, EventArgs e)
        {
            CardEnabled?.Invoke(this, false);
            Navigation.PopModalAsync(true);
        }
    }
}