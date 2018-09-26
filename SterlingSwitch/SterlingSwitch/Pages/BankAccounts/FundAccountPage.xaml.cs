using Newtonsoft.Json;
using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.Repository;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.BankAccounts
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FundAccountPage : SwitchMasterPage
    {
		public FundAccountPage (string accountNumber)
		{
			InitializeComponent ();
            AccountToFund = accountNumber;
            Setup();
        }

        string BaseURL = URLConstants.switchAPINewBaseURL;
        Customer customer = GlobalStaticFields.Customer;
        ApiRequest apiRequest = new ApiRequest();
        private List<string> accountList { get; set; }
        private string fromAccount { get; set; }

        private string card_no { get; set; }
        private string cvv { get; set; }
        private string expiry_year { get; set; }
        private string expiry_month { get; set; }
        private string pin { get; set; }
        private string amount { get; set; }
        private string AccountToFund { get; set; }
        private string CardType { get; set; }
        string CardPan { get; set; }

        List<string> AllCustomerCards = new List<string>();


        private void Setup()
        {
            var methodItems = new List<string>() { "From Card", "From Account" };
            MethodPicker.ItemsSource = methodItems;
            MethodPicker.SelectedIndex = 1;

            LoadCards();
            LoadAccounts();
            AllCustomerCards = new List<string>();
            AllCustomerCards.Add("Other Card");
        }

        private void LoadAccounts()
        {
            if (customer.ListOfAllAccounts != null && customer.ListOfAllAccounts.Count > 0)
            {
                List<string> acc = new List<string>();
                foreach (var item in customer.ListOfAllAccounts)
                {
                    acc.Add(item.AccountBalanceAccountType);
                }

                AccountPicker.ItemsSource = acc;
            }
        }

        private void LoadCards()
        {
            AllCustomerCards = new List<string>();

            foreach (var item in customer.MyCards)
                AllCustomerCards.Add(item.MaskedPan);

            AllCustomerCards.Add("Other Card");

            CardPicker.ItemsSource = AllCustomerCards;
        }

        private string ConvertMobile234(string mobile)
        {
            try
            {
                mobile = mobile.Replace("+", "");
                if (mobile.Length == 13 && mobile.StartsWith("234"))
                {
                    return mobile;
                }
                if (mobile.Length >= 10)
                {
                    mobile = "234" + mobile.Substring(mobile.Length - 10, 10);
                    return mobile;
                }
            }
            catch (Exception)
            {
                ;
            }

            return mobile;
        }

        private void ShowWalletAccountView()
        {
            AccountPicker.IsVisible = true;
            CardToDebitView.IsVisible = false;
        }

        private void ShowOwnCardView()
        {
            AccountPicker.IsVisible = false;
            CardToDebitView.IsVisible = true;
        }

        private async void GetAllCards()
        {

            //string url = "https://pass.sterlingbankng.com/PayphoneApi/api/Payphone"; //Test URL

            //// string url = "http://172.25.31.202:815"; //Live URL


            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("Appid", AppID);

            //if (string.IsNullOrWhiteSpace(customer.PhoneNumber))
            //    customer.PhoneNumber = "2348098674523";

            //customer.PhoneNumber = ConvertMobile234(customer.PhoneNumber);

            //var model = new GetCardModel()
            //{
            //    subscriberId = customer.PhoneNumber,
            //    appid = AppID
            //};

            //string json = JsonConvert.SerializeObject(model);

            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            //try
            //{
            //    await BusinessLogic.LogExceptionOrRemarkAsync(json, "Initialized Get Other Bank Card Request.");

            //    var response = await client.PostAsync(url, content);

            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace(@"\", "");
            //        jsonString = jsonString.Replace("\"{", "{");
            //        jsonString = jsonString.Replace("}\"", "}");
            //        jsonString = jsonString.Replace('\"', '"');
            //        var data = JsonConvert.DeserializeObject<AllCardsMethod>(jsonString);

            //        foreach (var t in data.PaymentMethods)
            //            AllCards.Add(t);

            //        LoadCards();
            //    }
            //    else
            //    {
            //        //MessageDialog.Show("Fund Account", "Error getting cards from other bank, please try again.", DialogType.Error, "OK");
            //        //MessageDialog.Show("Fund Account", "Error getting cards from other bank, please try again.", DialogType.Error, "OK");
            //        await BusinessLogic.LogExceptionOrRemarkAsync(response.ToString(), "Error getting cards from other bank, please try again.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //MessageDialog.Show("Fund Account", "Error getting cards from other bank, please try again.", DialogType.Error, "OK");
            //    //MessageDialog.Show("Fund Account", "Error getting cards from other bank, please try again.", DialogType.Error, "OK");
            //    await BusinessLogic.LogExceptionOrRemarkAsync(ex.ToString(), "Error getting cards from other bank, please try again.");
            //}
        }

        private async void FundWithAccount()
        {
            if (string.IsNullOrEmpty(fromAccount))
            {
                MessageDialog.Show("Fund Account", "Please Select an Account Number to transfer from", DialogType.Info, "OK", null);
                return;
            }
            else if (string.IsNullOrEmpty(AccountToFund))
            {
                MessageDialog.Show("Fund Account", "Please Enter Wallet Account to Credit.", DialogType.Info, "OK", null);
                return;
            }
            else if (string.IsNullOrEmpty(amount))
            {
                MessageDialog.Show("Fund Account", "Please Enter Fund Amount", DialogType.Info, "OK", null);
                return;
            }

            var pd = await ProgressDialog.Show("Please wait...");

            var model = new SterlingToWallet()
            {
                amt = amount.ToString(),
                frmacct = fromAccount,
                toacct = AccountToFund,
                paymentRef = "808080",
                Referenceid = Utilities.GenerateReferenceId(),
                remarks = "Fund wallet from my account.",
                RequestType = "110",
                tellerid = "1111",
                Translocation = GlobalStaticFields.GetUserLocation
            };
            
            string endpoint = "Spay/SBPT24txnRequest";
            string url = BaseURL + endpoint;

            try
            {
                var response = await apiRequest.Post(model, "", BaseURL, endpoint, "FundAccountPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    jsonString = jsonString.JsonCleanUp();
                    var json = JsonConvert.DeserializeObject<FundAccountResp>(jsonString);
                    //var val = JsonConvert.DeserializeObject<FundAccResp.Rootobject>(json.responsedata);
                    if (json.message.ToLower().Contains("approved"))
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Success", "Operation Completed Successfully", DialogType.Success, "OK", null);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Transaction Unsuccessful.", "Please verify your details and try again.", DialogType.Error, "OK", null);
                        await BusinessLogic.Log("", "Please verify your details and try again.", url, "", jsonString, "FundAccountPage");
                    }
                }
                else
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();
                    await pd.Dismiss();
                    MessageDialog.Show("Transaction Unsuccessful.", "Please verify your details and try again.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log("", "", url, "", jsonstring, "FundAccountPage");
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Transaction Unsuccessful.", "Unable to process your request at this time, please try again later.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Unable to process your request at this time, please try again later.", url, "", "", "FundAccountPage");
            }

        }

        private async void FundWithCard()
        {
            Device.BeginInvokeOnMainThread( async () =>
            {
                try
                {
                    Random r = new Random();
                    r.Next(1000, 9999);

                    var pd = await ProgressDialog.Show("Processing... Please Wait");

                    //string month ="", Date ="", year = "";
                    string referenceId = "00055" + DateTime.Now.ToString("yymmddHHmmss") + r.Next().ToString();

                    string mobile = customer.PhoneNumber;
                    string convertedNo = ConvertMobile234(mobile);

                    if (expiry_year.Contains("/"))
                        expiry_year = expiry_year.ToString().Remove(2, 1);

                    var fundacct = new DebitAnyBankCard()
                    {
                        amount = amount.ToString(),
                        currency = "NGN",
                        customerId = customer.Email,
                        cvv = cvv,
                        expiry_date = expiry_year,
                        pan = CardPan,
                        pin = pin,
                        Referenceid = referenceId,
                        RequestType = "200",
                        Translocation = GlobalStaticFields.GetUserLocation,
                        CreditAccount = AccountToFund
                    };

                    if (string.IsNullOrEmpty(fundacct.cvv))
                    {
                        MessageDialog.Show("Fund Account", "CVV is Required", DialogType.Info, "OK", null);
                        return;
                    }
                    else if (string.IsNullOrEmpty(fundacct.pan))
                    {
                        MessageDialog.Show("Fund Account", "Card Number is Required", DialogType.Info, "OK", null);
                        return;
                    }
                    else if (string.IsNullOrEmpty(fundacct.pin))
                    {
                        MessageDialog.Show("Fund Account", "Card Pin is Required", DialogType.Info, "OK", null);
                        return;
                    }
                    else if (string.IsNullOrEmpty(fundacct.expiry_date))
                    {
                        MessageDialog.Show("Fund Account", "Expiry Year and Month is Required", DialogType.Info, "OK", null);
                        return;
                    }
                    else if (string.IsNullOrEmpty(fundacct.amount.ToString()))
                    {
                        MessageDialog.Show("Fund Account", "Fund Amount is Required", DialogType.Info, "OK", null);
                        return;
                    }
                    else
                    {
                        string endpoint = "Spay/DebitAnyBankCard";
                        string url = BaseURL + endpoint;

                        try
                        {
                            var response = await apiRequest.Post(fundacct, "", BaseURL, endpoint, "FundAccountPage",true);

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var jsonString = await response.Content.ReadAsStringAsync();

                                if (!jsonString.Contains("invalid"))
                                {
                                    jsonString = jsonString.JsonCleanUp();
                                    var json = JsonConvert.DeserializeObject<FundAccountResp>(jsonString);
                                    if (json.message.ToLower().Contains("approved"))
                                    {
                                        await pd.Dismiss();
                                        Navigation.PushAsync(new Dashboard.Dashboard());
                                        MessageDialog.Show("Transaction Successful", "Transaction completed successfully.", DialogType.Success, "OK", null);
                                        await Navigation.PopAsync();
                                    }
                                    else if (json.responseCode == "T0")
                                    {
                                        await pd.Dismiss();
                                        await Navigation.PushAsync(new FundAccountOTPPage(fundacct, json.paymentId));
                                    }
                                    else if (json.responseCode == "S0")
                                    {
                                        // call method to debit visa
                                        var modelVISA = new DebitAnyBankCardVISA()
                                        {
                                            amount = amount.ToString(),
                                            cvv = cvv,
                                            expiry_date = expiry_year,
                                            paymentId = json.paymentId,
                                            eciFlag = "07",
                                            pan = CardPan,
                                            pin = pin,
                                            transactionId = Utilities.GenerateReferenceId(),
                                            CreditAccount = AccountToFund
                                        };

                                        endpoint = "Spay/DebitVISAcardOnly";
                                        url = BaseURL + endpoint;

                                        var rspVisa = await apiRequest.Post(modelVISA, "", BaseURL, endpoint, "FundAccountPage",true);

                                        if (rspVisa.StatusCode == HttpStatusCode.OK)
                                        {
                                            var respString = await rspVisa.Content.ReadAsStringAsync();
                                            respString = respString.JsonCleanUp();

                                            var jsonVISA = JsonConvert.DeserializeObject<FundAccountResp>(respString);

                                            if (jsonVISA.message.ToLower().Contains("approved") || jsonVISA.message.ToLower().Contains("successful"))
                                            {
                                                await pd.Dismiss();
                                                Navigation.PushAsync(new Dashboard.Dashboard());
                                                MessageDialog.Show("Success", "Transaction completed successfully.", DialogType.Error, "OK", null);
                                            }
                                            else
                                            {
                                                await pd.Dismiss();
                                                MessageDialog.Show("Error", "Unable to charge your card. Please try again later", DialogType.Error, "OK", null);
                                                await BusinessLogic.Log("", "Unable to charge your card. Please try again later", "", "**SENSITIVE**", "", "FundAccountPage");
                                            }
                                        }
                                        else
                                        {
                                            await pd.Dismiss();
                                            MessageDialog.Show("Error", "Unable to charge your card. Please try again later", DialogType.Error, "OK", null);
                                            await BusinessLogic.Log("", "Unable to charge your card. Please try again later", "", "**SENSITIVE**", "", "FundAccountPage");
                                        }
                                    }
                                    else
                                    {
                                        await pd.Dismiss();
                                        MessageDialog.Show("Error", "Unable to charge your card. Please try again later", DialogType.Error, "OK", null);
                                        await BusinessLogic.Log("", "Unable to charge your card. Please try again later", "", "**SENSITIVE**", "", "FundAccountPage");
                                    }
                                }
                                else
                                {
                                    await pd.Dismiss();
                                    MessageDialog.Show("Transaction Unsuccessful", "Please verify that the card details you entered and try again.", DialogType.Error, "OK", null);
                                    await BusinessLogic.Log("", "Please verify that the card details you entered and try again.", "", "**SENSITIVE**", "", "FundAccountPage");
                                }
                            }
                            else
                            {
                                await pd.Dismiss();
                                MessageDialog.Show("Error", "Unable to connect to the server at the moment. please try again later.", DialogType.Error, "OK", null);
                                await BusinessLogic.Log("", "Unable to connect to the server at the moment. please try again later.", "", "**SENSITIVE**", "", "FundAccountPage");
                            }
                        }
                        catch (Exception ex)
                        {
                            await pd.Dismiss();
                            MessageDialog.Show("Error", "Unable to charge your card. Please try again later", DialogType.Error, "OK", null);
                            await BusinessLogic.Log(ex.ToString(), "Unable to charge your card. Please try again later", "", "**SENSITIVE**", "", "FundAccountPage");
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            });

        }

        private void MethodPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MethodPicker.SelectedItem == null)
                return;

            if (MethodPicker.SelectedItem == "From Card")
            {
                ShowOwnCardView();
            }
            else if (MethodPicker.SelectedItem == "From Account")
            {
                ShowWalletAccountView();
            }
        }

        private void CardPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CardPicker.SelectedItem == null)
                return;

            if (CardPicker.SelectedItem == "Other Card")
            {
                CardToDebitTxt.IsVisible = true;
                return;
            }
            else
                CardToDebitTxt.IsVisible = false;

            var maskedPan = CardPicker.SelectedItem.Split('-')[0].Trim();

            CardType = customer.MyCards.Where(x => x.MaskedPan == maskedPan.Trim()).SingleOrDefault().CardType;
            CardPan = customer.MyCards.Where(x => x.MaskedPan == maskedPan).SingleOrDefault().CardPan;

        }

        private void ExpiryTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ExpiryTxt.Text.Length > 5)
                ExpiryTxt.Text = e.OldTextValue;

            if (ExpiryTxt.Text.Length == 2 && !ExpiryTxt.Text.Contains("/"))
                ExpiryTxt.Text += "/";
        }

        private async void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var method = MethodPicker.SelectedItem;
                amount = AmountTxt.Text;

                if (string.IsNullOrWhiteSpace(MethodPicker.SelectedItem))
                {
                    MessageDialog.Show("Fund Card", "Kindly select funding method.", DialogType.Error, "OK", null);
                    return;
                }

                if (string.IsNullOrWhiteSpace(amount))
                {
                    MessageDialog.Show("Fund Card", "Kindly enter the amount to fund.", DialogType.Error, "OK", null);
                    return;
                }

                if (MethodPicker.SelectedItem == "From Card")
                {
                    expiry_year = ExpiryTxt.Text.Replace("/", "");
                    cvv = CVVTxt.Text;
                    pin = PinTxt.Text;


                    if (CardPicker.SelectedItem == "Other Card")
                    {
                        CardPan = CardToDebitTxt.Text;
                    }
                    
                    if (string.IsNullOrWhiteSpace(CardPan))
                    {
                        MessageDialog.Show("Fund Card", "Kindly select the card to debit.", DialogType.Error, "OK", null);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(expiry_year))
                    {
                        MessageDialog.Show("Fund Card", "Kindly enter the card expiry date.", DialogType.Error, "OK", null);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(cvv))
                    {
                        MessageDialog.Show("Fund Card", "Kindly enter the card's CVV.", DialogType.Error, "OK", null);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(pin))
                    {
                        MessageDialog.Show("Fund Card", "Kindly enter the card's PIN.", DialogType.Error, "OK", null);
                        return;
                    }


                    MessageDialog.Show($"You are about to fund your Account {AccountToFund} with the sum of {amount}.", "Pleae confirm this transaction", DialogType.Info, "CONFIRM", FundWithCard, "CANCEL", null);
                   
                }
                else if (MethodPicker.SelectedItem == "From Account")
                {
                    string account = string.Empty;

                    account = customer.ListOfAllAccounts.FirstOrDefault(k => k.AccountBalanceAccountType == AccountPicker.SelectedItem).nuban;


                    if (string.IsNullOrWhiteSpace(account))
                    {
                        MessageDialog.Show("Fund Card", "Kindly select account to debit.", DialogType.Error, "OK", null);
                        return;
                    }

                    fromAccount = account;
                    FundWithAccount();

                }
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "Catching exceptions on submit buttom click", "", "**SENSITIVE**", "", "FundAccountPage");
            }
        }
    }

}