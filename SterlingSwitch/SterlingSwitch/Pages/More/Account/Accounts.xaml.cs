using SterlingSwitch.Helper;
using SterlingSwitch.Pages.More.Account.ViewModel;
using SterlingSwitch.Services;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.More
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Accounts : SwitchMasterPage
    {
        ListAccountViewModel vm;
        public string Phone = GlobalStaticFields.Customer.PhoneNumber;
        public Accounts ()
		{
            try
            {
                InitializeComponent();
                vm = new ListAccountViewModel(Navigation);
                this.BindingContext = vm;

                if (tabView.SelectedIndex == 0)
                    Resources["accounts"] = Application.Current.Resources["SelectedTabView"];
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
               // throw;
            }
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            List<ProfileListAccount> profileListAccounts = new List<ProfileListAccount>();

            if (!vm.IsAccountLoaded)
            {
                if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts.Count > 0)
                {
                    vm.CustomerAccounts.ReplaceRange(GlobalStaticFields.Customer.ListOfAllAccounts);

                    if(vm.CustomerAccounts?.Count  > 0)
                    {
                        foreach (var item in vm.CustomerAccounts)
                        {
                            var profileAcc = new ProfileListAccount
                            {
                                nuban =item.nuban,
                                AccountType = item.accountType,
                                BalanceWithCurrency = $"{Utilities.GetCurrency(item.currencyCode)}{item.balance}"
                            };

                            profileListAccounts.Add(profileAcc);
                            
                        }
                    }

                    vm.profileListAccount.ReplaceRange(profileListAccounts);
                    vm.IsAccountLoaded = true;
                }
                else
                {
                    await vm.GetCustomerAccountsbyPhoneNumber(Phone);
                }
            }
        }

        protected void Account_Tapped(object sender, EventArgs e)
        {
            try
            {
                tabView.SelectedIndex = 0;
                ResetSelectedTab();
                Resources["accounts"] = Application.Current.Resources["SelectedTabView"];
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                //throw;
            }
        }

        private void ResetSelectedTab()
        {
            Resources["accounts"] = Application.Current.Resources["UnSelectedTabView"];
            Resources["cards"] = Application.Current.Resources["UnSelectedTabView"];
        }

        protected void Card_tapped(object sender, EventArgs e)
        {
            try
            {
                
                //tabView.SelectedIndex = 1;
                //ResetSelectedTab();
                //Resources["cards"] = Application.Current.Resources["SelectedTabView"];
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
            }
        }

        private void Backbtn_Tapped(object sender, EventArgs e)
        {
            try
            {
                Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
               
            }
        }

        private void AddNewAccount_Tapped(object sender, EventArgs e)
        {

        }
    }
}