using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SterlingSwitch.Pages.BillPayments;
using SterlingSwitch.Pages.BillsPayment;
using SterlingSwitch.Pages.Investments;
using SterlingSwitch.Pages.Specta;
using SterlingSwitch.Pages.Specta.Service;
using SterlingSwitch.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.QuickActions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuickActions : SwitchMasterPage
    {
		public QuickActions ()
		{
			InitializeComponent ();
            GetPaymentCategories();
        }

        private async void GetPaymentCategories()
        {
            await Task.Run(() => SpectaService.GetSpectaDropdoown());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           // Main.TranslateTo(0, 0, 2000, Easing.BounceIn);
        }

        private async void investmentTap_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new LandingPage());
           // await Navigation.PopModalAsync(true);
        }

        private async void Withdrawal_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new CardlessWithdrawals.CardlessWithdrawView());
          //  await Navigation.PopModalAsync(true);
        }

        private async void AirtimeandData_Tapped(object sender, EventArgs e)
        {
            try{
                await App.Current.MainPage.Navigation.PushAsync(new AirtimeAndData.AirtimeAndData(), true);
               // await Navigation.PopModalAsync(true);
            }
            catch(Exception ee){
                var ex = ee.Message;
            }

        }

        private async void Bill_OnTapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new PayBillsPage(), true);
          //  await Navigation.PopModalAsync(true);
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
    }
}