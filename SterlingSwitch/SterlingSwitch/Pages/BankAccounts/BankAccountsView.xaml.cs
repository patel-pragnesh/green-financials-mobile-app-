using CarouselView.FormsPlugin.Abstractions;
using SterlingSwitch.Models;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace SterlingSwitch.Pages.BankAccounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BankAccountsView : SwitchMasterPage
    {
        BankAccountsViewModel vm;
        public string Phone = GlobalStaticFields.Customer.PhoneNumber;
        public BankAccountsView()
        {
            InitializeComponent();
            vm = new BankAccountsViewModel(Navigation);
            BindingContext = vm;
           

            if (tabView.SelectedIndex == 0)
            {
                bxaccount.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
            }
        }

      
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!vm.IsAccountLoaded)
            {
                vm.WalletLoading = true;
                if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts.Count > 0)
                {
                   // vm.CustomerAccounts.Clear();
                    vm.CustomerAccounts.ReplaceRange(GlobalStaticFields.Customer.ListOfAllAccounts);
                    vm.IsAccountLoaded = true;
                    if (vm.CustomerAccounts.Count > 1)
                        vm.CanSwipeLeft = true;

                    var account = vm.CustomerAccounts[0];
                    vm.SelectedAccount = account;
                    await vm.GetTransactionsByNuban(account.nuban, account.currencyCode);
                }
                else
                {
                    await vm.GetCustomerAccountsbyPhoneNumber(Phone);
                    if (vm.CustomerAccounts.Count > 0)
                    {
                        if (vm.CustomerAccounts.Count > 1)
                            vm.CanSwipeLeft = true;
                        var account = vm.CustomerAccounts[0];
                        vm.SelectedAccount = account;
                        await vm.GetTransactionsByNuban(account.nuban, account.currencyCode);
                    }
                }


                //Load Wallet
                await vm.GetCustomerWalletbyPhoneNumber(Phone);
                await vm.GetWalletTransactions();
            }

            if (GlobalStaticFields.Customer.MyCards != null && GlobalStaticFields.Customer.MyCards.Count > 0)
            {
                vm.CustomerCards.ReplaceRange(GlobalStaticFields.Customer.MyCards);
            }
        }


        private void SfTabView_TabItemTapped(object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {

        }

        private void SfTabView_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {

        }

        private void AccountTapped(object sender, EventArgs e)
        {
            // var obj = (StackLayout)sender;
            tabView.SelectedIndex = 0;
            ResetSelectedTab();
            bxaccount.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
         // Application.Current.Resources["accounts"] = Application.Current.Resources["SelectedTabView"];

        }
        private void CardsTapped(object sender, EventArgs e)
        {
            //  var obj = (StackLayout)sender;
            tabView.SelectedIndex = 1;
            ResetSelectedTab();
          //  Application.Current.Resources["cards"] = Application.Current.Resources["SelectedTabView"];
            bxcard.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
        }
        private void WalletTapped(object sender, EventArgs e)
        {
            //var obj = (StackLayout)sender;
            tabView.SelectedIndex = 2;
            ResetSelectedTab();
           // Application.Current.Resources["wallets"] = Application.Current.Resources["SelectedTabView"];
            bxwallet.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
        }

        void ResetSelectedTab()
        {
          
            bxaccount.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
            bxcard.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
            bxwallet.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
        }
    }
}