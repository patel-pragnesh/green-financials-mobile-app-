using Newtonsoft.Json;
using SterlingSwitch.Extensions;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.BankAccounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FundAccountOTPPage : SwitchMasterPage
    {
        public FundAccountOTPPage(DebitAnyBankCard model, string paymentId)
        {
            InitializeComponent();
            fundModel = model;
            paymentID = paymentId;
            dbcOTPMOdel = new DebitAnyBankCardWithOTP();
        }

        public string paymentID { get; set; }
        string BaseURL = URLConstants.switchAPINewBaseURL;
        ApiRequest apiRequest = new ApiRequest();
        DebitAnyBankCard fundModel;
        DebitAnyBankCardWithOTP dbcOTPMOdel;

        private async void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(otpTxt.Text))
            {
                var pd = await ProgressDialog.Show("Please wait...");

                string endpoint = "Spay/DebitAnyBankCardWithOTP";
                string url = BaseURL + endpoint;

                try
                {
                    dbcOTPMOdel = new DebitAnyBankCardWithOTP()
                    {
                        pin = fundModel.pin,
                        expiry_date = fundModel.expiry_date,
                        cvv2 = fundModel.cvv,
                        pan = fundModel.pan,
                        otp = otpTxt.Text.Trim(),
                        CreditAccount = fundModel.CreditAccount,
                        paymentId = paymentID,
                        amount = fundModel.amount.ToString()
                    };


                    var response = await apiRequest.Post(dbcOTPMOdel, "", BaseURL, endpoint, "FundAccountOTPPage", true);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        await pd.Dismiss();
                        var jsonString = await response.Content.ReadAsStringAsync();
                        jsonString = jsonString.JsonCleanUp();
                        var responseObj = JsonConvert.DeserializeObject<FundAccountOTPResponse>(jsonString);
                        if(responseObj.message == "Successful")
                        {
                            MessageDialog.Show("Success", $"Account funding of {responseObj.amount} was successful.", DialogType.Success, "OK", null);
                            Navigation.PushAsync(new Dashboard.Dashboard());
                        }
                        else { MessageDialog.Show("Failure", "Sorry we are unable to process your request at the moment. Kindly try again later.", DialogType.Error, "DISMISS", null); }
                       

                    }
                    else
                    {
                        await pd.Dismiss();
                        var jsonString = await response.Content.ReadAsStringAsync();
                        MessageDialog.Show("Error", "Sorry, We are unable to credit recipient's account at the moment. Kindly try again later", DialogType.Error, "OK", null);
                        await BusinessLogic.Log("", "", url, "**SENSITIVE**", jsonString, "FundAccountOTPPage");
                    }
                }
                catch (Exception ex)
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Error", "Sorry, We are unable to credit recipient's account at the moment. Kindly try again later", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(ex.ToString(), "", url, "**SENSITIVE**", "", "FundAccountOTPPage");
                }
            }
            else
            {
                MessageDialog.Show("Fund Account", "Kindly enter the OTP sent to your phone number and/or email address to continue", DialogType.Info, "OK", null);
            }
        }
    }


    public class FundAccountOTPResponse
    {
        public string transactionIdentifier { get; set; }
        public string token { get; set; }
        public string tokenExpiryDate { get; set; }
        public string panLast4Digits { get; set; }
        public string cardType { get; set; }
        public string message { get; set; }
        public string amount { get; set; }
        public string transactionRef { get; set; }
    }

}