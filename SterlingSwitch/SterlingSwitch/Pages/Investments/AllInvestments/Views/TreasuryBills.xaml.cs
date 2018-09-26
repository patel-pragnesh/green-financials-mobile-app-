using SterlingSwitch.Pages.Investments.TreasuryBills;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.AllInvestments.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TreasuryBills : ContentView
	{
		public TreasuryBills ()
		{
			InitializeComponent ();
		}

	    private void TreasuryListView_ItemTapped(object sender, ItemTappedEventArgs e)
	    {
	        var item = (ParthianInnerArrayOfBillDetails)e.Item;
	        Navigation.PushAsync(new SellTreasuryBillsPage(item));
	        ((ListView) sender).SelectedItem = null;
	    }
	}
}