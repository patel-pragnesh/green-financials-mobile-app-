using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.WalletAllTransaction.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletAllTransactionView : ContentView
	{
		public WalletAllTransactionView ()
		{
			InitializeComponent ();
		}
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e == null)
                return;
            var obj = (ListView)sender;
            obj.SelectedItem = null;
        }
    }
}