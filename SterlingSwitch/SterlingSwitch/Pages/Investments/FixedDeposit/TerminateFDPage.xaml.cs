using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.FixedDeposit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TerminateFDPage : SwitchMasterPage
    {
        FixedDepositValue MyDeposit;
        ApiRequest apirequest = new ApiRequest();
        public TerminateFDPage (FixedDepositValue depositValue)
		{
			InitializeComponent ();
            MyDeposit = depositValue;

            LoadDetails();
		}

        private void LoadDetails()
        {
            try
            {
                BillAmountTxt.Text = MyDeposit.Amount;
                MaturityTxt.Text = MyDeposit.MATURITY_DATE;
                ValueTxt.Text = MyDeposit.MATURITY_VALUE;
            }
            catch (System.Exception ex) 
            {             
                string log = ex.Message;               
            }
        }

        private void PopPage()
        {
            Navigation.PopAsync();
        }

        private async void btnTerminate_Clicked(object sender, System.EventArgs e)
        {
            MessageDialog.Show("Transaction Confirmation", $"You are about to terminate your fixed deposit. ", DialogType.Question, "Do you want to Proceed ?",
            DoTerminate, "Cancel", null);
        }

        private async void DoTerminate()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
                try
                {
                    if (string.IsNullOrEmpty(MyDeposit.Reference))
                    {
                        MessageDialog.Show("OOPS", "Please select an investment to terminate", DialogType.Error, "OK", null);
                        return;
                    }
                    var arrangememtId = MyDeposit.Reference;

                    var model = new TerminateFixedDepositModel()
                    {
                        ReferenceID = DateTime.Now.Year.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Millisecond.ToString(),
                        arrangementid = MyDeposit.Reference,
                        RequestType = 925.ToString()
                    };

                    var response = await apirequest.PostIBS<TerminateFixedDepositModel>(model, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/IBSBridge", "FixedDepositPage");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (content.Contains("00"))
                        {
                            var status = new ChangeInvestmentStatus()
                            {
                                customerid = GlobalStaticFields.Customer.CustomerId,
                                Ref = arrangememtId,
                                response = "",
                                amount = "",
                                CURRENCY = "",
                                FIXED_RATE = "",
                                message = "",
                                NUBAN = "",
                                responseCode = "",
                                TERM = ""
                            };
                            var resp = await apirequest.Post<ChangeInvestmentStatus>(status, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/ChangeInvestStatus", "TerminateFixedDepositPage");
                            await pd.Dismiss();
                            var statusResponse = await resp.Content.ReadAsStringAsync();
                            if (statusResponse.Contains("1"))
                            {
                                MessageDialog.Show("SUCCESS", "You have successfully terminated your fixed deposit. Your account will be credited shortly.", DialogType.Error, "OK", null);
                            }
                            else
                            {
                                MessageDialog.Show("OOPS", "Sorry, we are unable to process your request at the moment. Please try again later.", DialogType.Error, "OK", null);
                                return;
                            }
                        }
                        else
                        {
                            await pd.Dismiss();
                            MessageDialog.Show("OOPS", "Sorry, we are unable to terminate your investment at the moment. Kinfly try again later", DialogType.Error, "OK", null);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await pd.Dismiss();
                    MessageDialog.Show("OOPS", "An error occured while processing your request. Kindly try again later", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(ex.Message, "Exception on Terminating Fixed Deposit", "IBS for Terminate FD, and ChangeInvestStatus on spay", "", "", "Terminate Fixed Deposit");
                }
            });
          
        }
    }
}