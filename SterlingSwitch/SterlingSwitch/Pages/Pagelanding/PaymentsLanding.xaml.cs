using FormsControls.Base;
//using SterlingSwitch.Pages.BillPayments;
using SterlingSwitch.Pages.BillsPayment;
using SterlingSwitch.Pages.BillsPayment.Service;
using SterlingSwitch.Pages.LocalTransfer;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin.TabView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Pagelanding
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentsLanding : SwitchMasterPage,IAnimationPage
    {
        public PaymentsLanding ()
		{
			InitializeComponent ();
            GetPaymentCategories();

            if (tabView.SelectedIndex == 0)
            {
                bxPay.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
            }
        }


        private void SendMoney_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SendMoney());
        }

        private async void ManageRecipient_Clicked(object sender, EventArgs e)
        {
            //if (GlobalStaticFields.ListOfTransferbeneficiaries != null)
            //{
            //   await Navigation.PushAsync(new ManageRecipientLocalTransfer());
            //}

            //else
            //{
            //    var pd = await ProgressDialog.Show("Beneficiaries, Please wait.");
            //    var mybeneficiary = await BusinessLogic.GetMyBeneficiaries(GlobalStaticFields.Customer.Email);
            //    await pd.Dismiss();
            //    if (mybeneficiary.Any())
            //    {                   
            //        GlobalStaticFields.ListOfTransferbeneficiaries = mybeneficiary;
            //        await Navigation.PushAsync(new ManageRecipientLocalTransfer());
            //    }
            //    else
            //    {
            //        MessageDialog.Show("Manage Recipients", "No recipient found", DialogType.Info, "OK", null);
            //    }
            //}

        }

        private async void GetPaymentCategories()
        {            
           await Task.Run(() => BillerServices.GetBillerCategories());
                          
        }

        void ResetSelectedTab()
        {
            bxPay.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
            bxSchedule.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
            bxRequest.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
        }

        private void CurrencySwap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pages.CurrencySwap.CurrencySwapPage());
        }

        private void CardToCard_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CardToCard.CardToCardPage());
        }

        private void billsPayment_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PayBillsPage());
        }

        private void ScheduleTapped(object sender, EventArgs e)
        {
            tabView.SelectedIndex = 2;
            ResetSelectedTab();
            bxSchedule.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
        }

        private void RequestTapped(object sender, EventArgs e)
        {
            tabView.SelectedIndex = 1;
            ResetSelectedTab();
            bxRequest.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
        }

        private void PayTapped(object sender, EventArgs e)
        {
            tabView.SelectedIndex = 0;
            ResetSelectedTab();
            bxPay.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
        }
    }
}