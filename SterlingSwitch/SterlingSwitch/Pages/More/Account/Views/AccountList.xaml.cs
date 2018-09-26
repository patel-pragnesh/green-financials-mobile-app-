using SterlingSwitch.Models;
using SterlingSwitch.Pages.More.Account.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.More.Account.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountList : ContentView
	{
        ListAccountViewModel vm;
		public AccountList ()
		{
			InitializeComponent ();
		}

        private void AccountListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           
        }

        private void AccountListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e == null)
                    return;
                var obj = (ListView)sender;

                var customerAccount = obj.SelectedItem as CustomerAccount;
                obj.SelectedItem = null;
                App.Current.MainPage.Navigation.PushAsync(new SelectedAccount(customerAccount));

            }
            catch (Exception ex)
            {

                //throw;
            }

        }
    }
}