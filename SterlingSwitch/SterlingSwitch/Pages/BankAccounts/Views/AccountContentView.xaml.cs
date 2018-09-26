using CarouselView.FormsPlugin.Abstractions;
using SterlingSwitch.PopUps;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin.TabView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.BankAccounts.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountContentView : ContentView
	{
        BankAccountsViewModel vm;
        public AccountContentView ()
		{
			InitializeComponent ();

           
		}
       // string acct { get; set; }

        private async void CarouselViewControl_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            try
            {
                var obj = (CarouselViewControl)sender;
                if (obj.BindingContext == null) return;
                vm = (BankAccountsViewModel)obj.BindingContext;

                if (!vm.IsAccountLoaded)
                    return;

                if (vm.IsAccountLoaded && vm.CustomerAccounts.Count > 1)
                {

                    var account = vm.CustomerAccounts[e.NewValue];

                    if (e.NewValue > 0)
                        vm.CanSwipeRight = true;
                    else
                        vm.CanSwipeRight = false;

                    if (e.NewValue == vm.CustomerAccounts.Count - 1)
                        vm.CanSwipeLeft = false;
                    else
                        vm.CanSwipeLeft = true;

                    vm.SelectedAccount = account;
                  //  acct = account.nuban;

                    await vm.GetTransactionsByNuban(account.nuban, account.currencyCode);
                }
            }
            catch (Exception ex)
            {

                
            }
        }

        private void CarouselViewControl_Scrolled(object sender, CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs e)
        {

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //if (e == null)
            //    return;
            //var list = (ListView)sender;
            //var obj = (RefinedTransactions)e.SelectedItem;
            //Navigation.PushAsync(new AllTransactions.DetailedTransactionView(obj), true);
            //list.SelectedItem = null;
        }



        private  void fund_Tapped(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(vm.SelectedAccount.nuban))
               vm.Navigation.PushAsync(new FundAccountPage(vm.SelectedAccount.nuban));
            else
                MessageDialog.Show("OOPS", "No Account Selected. Kindly swipe between the accounts to select an account to fund", DialogType.Info, "OK", null);
        }

        private void CustomListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null)
                return;
            var obj = (RefinedTransactions)e.Item;
            ((ListView)sender).SelectedItem = null;
           //Navigation.PushAsync(new AllTransactions.DetailedTransactionView(obj), true);
        }
    }
}