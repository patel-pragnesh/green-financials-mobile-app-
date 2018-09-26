using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Investments.ViewModel;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.Repository;
using SterlingSwitch.Services.RestServices;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.TreasuryBills
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfirmTreasuryBillsPage : ContentPage
	{
		public ConfirmTreasuryBillsPage (TreasuryBill bill)
		{
			InitializeComponent ();
            Setup(bill);
        }

        ApiRequest apiRequest = new ApiRequest();
        TreasuryBill treasuryBill;
        Customer customer = GlobalStaticFields.Customer;

        string BaseURL = URLConstants.SwitchApiLiveBaseUrl;
        //string BaseURL = URLConstants.SwitchApiTestBaseUrl;

        public string AccountNumber { get; set; }

        private void Setup(TreasuryBill bill)
        {
            treasuryBill = bill;
            BillAmountTxt.Text = Utilities.GetCurrency("NGN") + bill.Price.ToString("N");
            DurationTxt.Text = $"{bill.Tenor} Days";
            GetAccounts();
            ComputeValue();
            AccountPicker.RefreshContent = ReloadItems;

        }

        private void GetAccounts()
        {
            if (customer.ListOfAllAccounts != null && customer.ListOfAllAccounts.Count > 0)
            {
                List<string> acc = new List<string>();
                foreach (var item in customer.ListOfAllAccounts)
                {
                    acc.Add(item.AccountNumberWithBalance);
                }

                AccountPicker.ItemsSource = acc;
            }
        }

        private async void ComputeValue()
        {
            var pd = await ProgressDialog.Show("Getting value at maturity, please wait.");

            ComputeMaturityModel tbillModel = new ComputeMaturityModel()
            {
                Referenceid = Utilities.GenerateReferenceId(),
                RequestType = 125,
                Translocation = GlobalStaticFields.GetUserLocation,
                PhoneNumber = "08036083928",
                Amount = treasuryBill.Price.ToString(),
                InstrumentsID = treasuryBill.InstrumentTypeId.ToString()
            };

            var BaseURL = URLConstants.SwitchApiLiveBaseUrl;
            var endpoint = "Spay/computeParthiansimpleInvestReq";

            try
            {
                var response = await apiRequest.Post(tbillModel, "", BaseURL, endpoint, "ConfirmTreasuryBillsPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<SpayResponse>(jsonString);

                    if (result.responsedata != null)
                    {
                        var maturity = JsonConvert.DeserializeObject<MaturityModel>(result.responsedata);
                        ValueTxt.Text = Utilities.GetCurrency("NGN") + maturity.ValueAtMaturity.ToString("N");
                    }

                }

                await pd.Dismiss();
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                string log = ex.ToString();
                await BusinessLogic.Log(ex.ToString(), "Excepton on getting value at maturity", BaseURL + endpoint, "", "", "ConfirmTreasuryBillsPage");
            }
        }

        private void CallPathian()
        {
            Device.BeginInvokeOnMainThread(async() => 
            {
                try
                {
                    if (GlobalStaticFields.UserCreationOnParthian == 0)
                    {
                        var createCustomer = await CreateParthianAccount();

                        if (createCustomer == "00") //success
                        {
                            GlobalStaticFields.UserCreationOnParthian = 1;
                            await UpdateUserInfo(GlobalStaticFields.Customer.Email);

                            InvestInParthian();
                        }
                        else if (!createCustomer.Contains("00"))//failure
                        {
                            MessageDialog.Show("Error", "Unable to create your account with Parthian." + createCustomer + ". Please try again", DialogType.Error, "OK", null);
                        }
                    }
                    else
                    {
                        InvestInParthian();
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.Show("Error", "Unable to buy Treasury Bills at this time, please try again.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(ex.ToString(), "Unable to buy Treasury Bills at this time, please try again", "", "", "", "ConfirmTreasuryBillsPage");
                }
            });

        }

        public async Task<string> CreateParthianAccount()
        {
            var pd = await ProgressDialog.Show("Creating Your Treasury Bills Account...");
            string respon = "";
            string endpoint = "Spay/CreateParthianCust";
            string url = BaseURL + endpoint;

            var payload = new CreateParthianUser()
            {
                Referenceid = Utilities.GenerateReferenceId(),
                RequestType = 119,
                Translocation = GlobalStaticFields.GetUserLocation,
                FirstName = GlobalStaticFields.Customer.FirstName,
                LastName = GlobalStaticFields.Customer.LastName,
                Email = GlobalStaticFields.Customer.Email,
                PhoneNumber = GlobalStaticFields.Customer.PhoneNumber,
                AccountNo = AccountNumber
            };
            
            try
            {
                var response = await apiRequest.Post(payload, "", BaseURL, endpoint, "ConfirmTreasuryBillsPage");
                var responseString = await response.Content.ReadAsStringAsync();

                await BusinessLogic.Log("", "", url, "", responseString, "");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var getAllResult = await response.Content.ReadAsStringAsync();
                    var firstResponse = JsonConvert.DeserializeObject<SpayResponse>(getAllResult);

                    if (firstResponse.message.Contains("Approved"))
                        respon = "00";
                    else if (firstResponse.responsedata.Contains("already"))
                        respon = "00";
                    else
                        respon = "x01";
                }
                else
                {
                    var objError = JsonConvert.DeserializeObject<ExceptionMessagesFromAPI>(responseString);
                    respon = "x99::" + objError.ExceptionMessage.Substring(0, 20);
                }

                await pd.Dismiss();
                return respon;
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                await BusinessLogic.Log(ex.ToString(), "exception calling end point", url, "", "", "ConfirmTreasuryBillsPage");
                string log = ex.ToString();
                return "x01";
            }
        }

        public async Task UpdateUserInfo(string email)
        {
            var pd = await ProgressDialog.Show("Updating your account...");

            try
            {
                var obj = new UpdateParthian()
                {
                    UserID = email
                };

                string endpoint = "Switch/UpdateParthian";
                string url = BaseURL + endpoint;


                var response = await apiRequest.Post(obj, "", BaseURL, endpoint, "ConfirmTreasuryBillsPage");
                var breakdown = await response.Content.ReadAsStringAsync();

                await BusinessLogic.Log("", "", url, "", breakdown, "");

            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.ToString(), "Exception on Updating Parthian", "", "", "", "ConfirmTreasuryBillsPage");
            }

            await pd.Dismiss();
        }

        public async void InvestInParthian()
        {
            var pd = await ProgressDialog.Show("Purchasing Treasury Bills...");
            string endpoint = "Spay/BuyParthiansimpleInvestReq";
            string url = BaseURL + endpoint;

            BuyParthianBills pthbill = new BuyParthianBills()
            {
                Amount = treasuryBill.Price.ToString(),
                InstrumentsID = treasuryBill.InstrumentTypeId.ToString(),
                PhoneNumber = GlobalStaticFields.Customer.PhoneNumber,
                Referenceid = Utilities.GenerateReferenceId(),
                RequestType = 124,
                TransactionReference = Utilities.GenerateReferenceId(),
                Translocation = GlobalStaticFields.GetUserLocation,
                Nuban = AccountNumber
            };

            try
            {
                var result = await apiRequest.Post(pthbill, "", BaseURL, endpoint, "ConfirmTreasuryBillsPage");
                var extractthis = await result.Content.ReadAsStringAsync();
                

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var getAllResult = await result.Content.ReadAsStringAsync();
                    var firstResponse = JsonConvert.DeserializeObject<SpayResponse>(getAllResult);
                    if (firstResponse.response == "00")
                    {
                        //var inv = JsonConvert.DeserializeObject<SuccessfulParthianInvestment>(firstResponse.responsedata);

                        //The response from pathian changed so we are checking if the response contains success
                        if(firstResponse.responsedata.ToLower().Contains("success"))
                        {
                            //success
                            await pd.Dismiss();
                            MessageDialog.Show("Treasury Bills", $"You have successfully purchased Treasury Bills of {BillAmountTxt.Text} with maturity value of {ValueTxt.Text} in {treasuryBill.Tenor} days.", DialogType.Success, "OK", null);
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            //Failed
                            await pd.Dismiss();
                            //inv.RespDesc = firstResponse.responsedata;
                            //inv.RespCode = firstResponse.response;
                            MessageDialog.Show("Error", "Something went wrong during your purchase.", DialogType.Error, "OK", null);
                            await BusinessLogic.Log($"Something went wrong during your purchase. {firstResponse.responsedata}", "exception calling API ", endpoint, "", "", "ConfirmTreasuryBillsPage");
                        }
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Error", "Something went wrong during your purchase.", DialogType.Error, "OK", null);
                        await BusinessLogic.Log($"Something went wrong during your purchase. {firstResponse.responsedata}", "exception calling API ", endpoint, "", "", "ConfirmTreasuryBillsPage");
                    }
                }
                else
                {
                    //Status Code not OK
                    await pd.Dismiss();
                    var extract = await result.Content.ReadAsStringAsync();
                    var extractMore = JsonConvert.DeserializeObject<ExceptionMessagesFromAPI>(extract);
                    MessageDialog.Show("Error", "Something went wrong during your purchase.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(extractMore.ToString(), "exception calling API ", endpoint, "", "", "ConfirmTreasuryBillsPage");
                }
            }
            catch (Exception ee)
            {
                await pd.Dismiss();
                string log = ee.ToString();
                MessageDialog.Show("Error", $"Something went wrong during your purchase.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ee.ToString(), "exception calling API ", endpoint, "", "", "ConfirmTreasuryBillsPage");
            }

        }

        void ReloadItems()
        {
            //AccountPicker.ItemsSource = new List<string> { "09876", "12345" };
        }


        private void Dismiss(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Confirm(object sender, EventArgs e)
        {
            if (AccountPicker.SelectedItem == null)
            {
                MessageDialog.Show("Treasury Bills", "Kindly select an account to continue.", DialogType.Info, "OK", null);
                return;
            }

            string message = $"You are about to purchase Treasury Bill of {BillAmountTxt.Text} with maturity value of {ValueTxt.Text} in {treasuryBill.Tenor} days. Do you want to continue?";

            if (string.IsNullOrWhiteSpace(ValueTxt.Text))
                message = $"You are about to purchase Treasury Bill of {BillAmountTxt.Text}. Do you want to continue?";

            MessageDialog.Show("Treasury Bills", message, DialogType.Question, "Proceed",
              () => 
              {
                  Device.BeginInvokeOnMainThread(async () =>
                  {
                      if (GlobalStaticFields.Customer.IsTPin)
                      {
                          var msg = $"Maturity value:{ValueTxt.Text}";
                          var ValidateTreasureBillTPIN = new PopUps.VerifyPinPage("Confirma Treasure Bill", BillAmountTxt.Text, "NGN", msg);
                          ValidateTreasureBillTPIN.Validated += ValidateTreasureBillTPIN_Validated;
                          await Navigation.PushAsync(ValidateTreasureBillTPIN);

                      }
                      else
                      {
                          CallPathian();
                      }
                  });
              },"Cancel", null);

        }

        private void ValidateTreasureBillTPIN_Validated(object sender, bool e)
        {
            CallPathian();
        }

        private void AccountPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountPicker.SelectedItem == null)
                return;

            var account = AccountPicker.SelectedItem;
            AccountNumber = customer.ListOfAllAccounts.Where(x => x.AccountNumberWithBalance == account).SingleOrDefault().nuban;
        }
    }
}