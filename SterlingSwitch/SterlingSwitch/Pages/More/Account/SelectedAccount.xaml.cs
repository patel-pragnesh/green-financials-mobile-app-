using SterlingSwitch.Models;
using SterlingSwitch.Pages.More.Account.ViewModel;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.More.Account
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectedAccount : SwitchMasterPage
    {
        SelectedAccountViewModel vm;
        public SelectedAccount (CustomerAccount customerAccount)
		{
            try
            {
                InitializeComponent();
                vm = new SelectedAccountViewModel(Navigation);
                BindingContext = vm;

               if (customerAccount == null)
                  return;

                vm.CustomerAccounts = customerAccount;
            }
            catch (Exception ex)
            {

                
            }
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (vm.CustomerAccounts == null)
                await Navigation.PopAsync();
        }

        private void StatementHistory_ItemTapped(object sender, Custom.Controls.ExtendedLabelTappedEvent e)
        {
            try
            {

            }
            catch (Exception ex)
            {


            }
        }

        private void AccountVerification_ItemTapped(object sender, Custom.Controls.ExtendedLabelTappedEvent e)
        {
            try
            {

            }
            catch (Exception ex)
            {


            }
        }

        private void CorrespondingUSD_ItemTapped(object sender, Custom.Controls.ExtendedLabelTappedEvent e)
        {
            try
            {

            }
            catch (Exception ex)
            {


            }
        }

        private void CorrespondingGBP_ItemTapped(object sender, Custom.Controls.ExtendedLabelTappedEvent e)
        {
            try
            {

            }
            catch (Exception ex)
            {


            }
        }

        private void CorresponsingEUR_ItemTapped(object sender, Custom.Controls.ExtendedLabelTappedEvent e)
        {
            try
            {

            }
            catch (Exception ex)
            {


            }
        }
    }
}