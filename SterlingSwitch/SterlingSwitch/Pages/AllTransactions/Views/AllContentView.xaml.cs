using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.AllTransactions.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AllContentView : ContentView
	{
        
		public AllContentView ()
		{
			InitializeComponent ();
		}

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null)
                return;
            var obj = (RefinedTransactions)e.Item;
            ((ListView)sender).SelectedItem = null;
           //await Navigation.PushAsync(new AllTransactions.DetailedTransactionView(obj), true);
          await  Application.Current.MainPage.Navigation.PushAsync(new DetailedTransactionView(obj));
           
        }
    }
}