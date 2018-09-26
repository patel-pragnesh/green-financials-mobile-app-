using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.TreasuryBills.view
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TreasuryBillView : ContentView
	{
		public TreasuryBillView ()
		{
			InitializeComponent ();
		}

        private void TbillsBtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new TreasuryBillsPage());
        }
    }
}