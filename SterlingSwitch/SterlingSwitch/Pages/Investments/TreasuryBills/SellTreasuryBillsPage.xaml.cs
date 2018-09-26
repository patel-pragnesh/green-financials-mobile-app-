using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.TreasuryBills
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SellTreasuryBillsPage : ContentPage
	{
		public SellTreasuryBillsPage (ParthianInnerArrayOfBillDetails bill)
		{
			InitializeComponent ();
            treasuryBill = bill;
            LoadBill();
		}

        ApiRequest apiRequest = new ApiRequest();
        ParthianInnerArrayOfBillDetails treasuryBill;

        private void LoadBill()
        {
            MaturityTxt.Text = treasuryBill.MaturityDate.ToString("MMM dd, yyyy");
            //RateTxt.Text = treasuryBill.InterestRate + "%";
            ValueTxt.Text = Utilities.GetCurrency("NGN") + treasuryBill.ValueAtMaturity.ToString("N2");
            BillAmountTxt.Text = Utilities.GetCurrency("NGN") + treasuryBill.AmountInvested.ToString("N2");
        }

        private void SellTBills()
        {
            Device.BeginInvokeOnMainThread(async() => 
            {
                var pd = await ProgressDialog.Show("Selling your Treasury Bills, please wait...");

                var sellingTbills = new SellParthian()
                {
                    Amount = treasuryBill.AmountInvested.ToString(),
                    InstrumentsID = treasuryBill.InstrumentId.ToString(),
                    PhoneNumber = GlobalStaticFields.Customer.PhoneNumber,
                    Referenceid = Utilities.GenerateReferenceId(),
                    RequestType = 123,
                    TransactionReference = Utilities.GenerateReferenceId(),
                    Translocation = GlobalStaticFields.GetUserLocation
                };

                string BaseURL = URLConstants.SwitchApiLiveBaseUrl;
                string endpoint = "Spay/SellParthiansimpleInvestReq";
                string url = BaseURL + endpoint;

                try
                {
                    var response = await apiRequest.Post(sellingTbills, "", BaseURL, endpoint, "SellTreasuryBillsPage");
                    var result = await response.Content.ReadAsStringAsync();

                    await pd.Dismiss();

                    if (result.Contains("00"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessageDialog.Show("Transaction Successful", "You have successfully sold your Treasury Bill.", DialogType.Success, "OK", null);
                        });
                    }
                    else
                    {
                        MessageDialog.Show("Error", "Selling your Treasury Bill was not successful.", DialogType.Error, "OK", null);

                    }
                }
                catch (Exception ex)
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Error", "Selling your Treasury Bill was not successful.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(ex.ToString(), "exception calling sell parthian", url, "", "", "SellTreasuryBillsPage");
                }
            });
            
        }

        private void SellTreasuryBill(object sender, EventArgs e)
        {
            string message = $"You are about to sell your Treasury Bill earlier than it's maturity date of {MaturityTxt.Text}. Do you want to continue?";

            if(DateTime.Now.Date >= treasuryBill.MaturityDate.Date)
                message = $"You are about to sell your Treasury Bill. Do you want to continue?";

            MessageDialog.Show("Treasury Bills", message, DialogType.Question, "Proceed",
                ()=> {
                    Device.BeginInvokeOnMainThread(() => {
                        if (GlobalStaticFields.Customer.IsTPin)
                        {
                            var SelltreasuryBillVerifyPIN = new PopUps.VerifyPinPage("Confirmation", Convert.ToDecimal(ValueTxt.Text).ToString("N2"), "NGN", null);
                            SelltreasuryBillVerifyPIN.Validated += SelltreasuryBillVerifyPIN_Validated;
                            Navigation.PushAsync(SelltreasuryBillVerifyPIN);
                        }
                        else
                        {
                            SellTBills();
                        }
                    });
                }, "Cancel", null);
        }

        private void SelltreasuryBillVerifyPIN_Validated(object sender, bool e)
        {
            SellTBills();
        }

        private async void Dismiss(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}