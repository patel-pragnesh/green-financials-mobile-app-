using System;
using System.Collections.Generic;
using SterlingSwitch.Pages.BillsPayment;
using SterlingSwitch.Pages.CardlessWithdrawals;
using SterlingSwitch.Pages.Investments;
using SterlingSwitch.Pages.Specta;
using SterlingSwitch.Templates;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.QuickActions
{
    public partial class QuickActionsV2 : SwitchMasterPage
    {
        public QuickActionsV2()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }

        private async void Bill_OnTapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new PayBillsPage(), true);
            //  await Navigation.PopModalAsync(true);
        }

        private async void AirtimeandData_Tapped(object sender, EventArgs e)
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new AirtimeAndData.AirtimeAndData(), true);
                // await Navigation.PopModalAsync(true);
            }
            catch (Exception ee)
            {
                var ex = ee.Message;
            }

        }

        private async void QuickLoan_Tapped(object sender, EventArgs e)
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new QuickLoan(), true);
            }
            catch (Exception ee)
            {
                var ex = ee.Message;
            }
        }

        private async void investmentTap_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new LandingPage(), true);
            // await Navigation.PopModalAsync(true);
        }

        private async void Withdrawal_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new CardlessWithdrawals.CardlessWithdrawView());
           // await App.Current.MainPage.Navigation.PushAsync(new CardlessWithdrawalSuccessful());
             
        }
    }
}
