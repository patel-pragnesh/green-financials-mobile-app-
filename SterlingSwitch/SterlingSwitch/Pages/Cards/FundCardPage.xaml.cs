using SterlingSwitch.Pages.Cards.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
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

namespace SterlingSwitch.Pages.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FundCardPage : SwitchMasterPage
    {
		public FundCardPage ()
		{
			InitializeComponent ();
            Setup();
        }

        FundWithCardModel cardModel;
        FundWithWalletModel walletdModel;
        FundWithAccountModel accountModel;

        ApiRequest apirequest = new ApiRequest();

        private void Setup()
        {
            //List<string> destinationItems = new List<string>() { "My Card", "Other Card" };
            //DestinationCardPicker.ItemsSource = destinationItems;

            List<string> methodItems = new List<string>() { "From Card", "From Account" };
            MethodPicker.ItemsSource = methodItems;

            List<string> cardCurrency = new List<string>() { "NGN", "USD", "GBP", "EUR" };
            CardCurrencyPicker.ItemsSource = cardCurrency;

            List<string> cards = new List<string>();

            foreach (var card in GlobalStaticFields.Customer?.MyCards)
                cards.Add(card.MaskedPan);

            CardPicker.ItemsSource = cards;

            LoadAccounts();
        }

        private void LoadAccounts()
        {
            if (GlobalStaticFields.Customer?.ListOfAllAccounts != null && GlobalStaticFields.Customer?.ListOfAllAccounts.Count > 0)
            {
                List<string> acc = new List<string>();
                foreach (var item in GlobalStaticFields.Customer?.ListOfAllAccounts)
                {
                    acc.Add(item.AccountNumberWithBalance);
                }

                AccountPicker.ItemsSource = acc;
            }
        }

        private async void FundCardWithWallet()
        {
            var pd = await ProgressDialog.Show("Processing...please wait!");

            string baseUrl = URLConstants.SwitchApiLiveBaseUrl;
            string endpoint = "Spay/debitWalletCreditSterlingCardReq";
            string url = baseUrl + endpoint;

            try
            {
                var response = await apirequest.Post(walletdModel, "", baseUrl, endpoint, "FundCardPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString.ToLower().Contains("success"))
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Success", "Transaction was successful", DialogType.Success, "OK", async() => { await Navigation.PopAsync(); }); 
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                        await BusinessLogic.Log(jsonString.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(response.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Fund  Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
            }

            
        }
        
        private async void FundCardWithCard()
        {
            var pd = await ProgressDialog.Show("Processing...please wait!");
            
            string baseUrl = URLConstants.SwitchApiLiveBaseUrl;
            string endpoint = "Spay/DebitAnyBankCardCreditSterlingCardOnly";
            string url = baseUrl + endpoint;


            try
            {
                var response = await apirequest.Post(cardModel, "", baseUrl, endpoint, "FundCardPage");
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString.ToLower().Contains("success"))
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Success", "Transaction was successful", DialogType.Success, "OK", async () => { await Navigation.PopAsync(); });
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                        await BusinessLogic.Log(jsonString.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(response.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
            }

        }

        private async void FundCardWithAccount()
        {
            var pd = await ProgressDialog.Show("Processing...please wait!");

            string baseUrl = URLConstants.SwitchApiLiveBaseUrl;
            string endpoint = "Spay/debitSterlingAcctCreditSterlingCardReq";
            string url = baseUrl + endpoint;
            
            try
            {
                var response = await apirequest.Post(accountModel, "", baseUrl, endpoint, "FundCardPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString.ToLower().Contains("success"))
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Success", "Transaction was successful", DialogType.Success, "OK", async () => { await Navigation.PopAsync(); });
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                        await BusinessLogic.Log(jsonString.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(response.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Fund Card", "Error funding card, please try again.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Error funding card, please try again.", url, "", "", "FundCardPage");
            }
        }

        private void ShowWalletAccountView()
        {
            AccountPicker.IsVisible = true;
            CardToDebitView.IsVisible = false;
        }

        private void ShowWOwnCardView()
        {
            AccountPicker.IsVisible = false;
            CardToDebitView.IsVisible = true;
        }

        private void MethodPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MethodPicker.SelectedItem == null)
                return;

            if (MethodPicker.SelectedItem == "From Card")
            {
                ShowWOwnCardView();
            }
            else if (MethodPicker.SelectedItem == "From Account")
            {
                ShowWalletAccountView();
            }
        }

        private void DestinationCardPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DestinationCardPicker.SelectedItem == "Other Card" && MethodPicker.SelectedItem == "From Card")
            //{
            //    GotoCardToCard();
            //}
            //else if (DestinationCardPicker.SelectedItem == "Other Card" && MethodPicker.SelectedItem == "From Account")
            //{
            //    GotoCardToCard();
            //}
            //else if (DestinationCardPicker.SelectedItem == "My Card" && MethodPicker.SelectedItem == "From Card")
            //{
            //    ShowWOwnCardView();
            //}
            //else if (DestinationCardPicker.SelectedItem == "My Card" && MethodPicker.SelectedItem == "From Account")
            //{
            //    ShowWalletAccountView();
            //}
        }

        private void ExpiryTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ExpiryTxt.Text.Length > 5)
                ExpiryTxt.Text = e.OldTextValue;

            if (ExpiryTxt.Text.Length == 2 && !ExpiryTxt.Text.Contains("/"))
                ExpiryTxt.Text += "/";
        }

        private async void SubmitBtn_Tapped(object sender, EventArgs e)
        {
            try
            {
                //Populate various models(Wallet, Account, Card)
                //var destination = DestinationCardPicker.SelectedItem;
                var method = MethodPicker.SelectedItem;
                //var account = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault(k => k.AccountNumberWithBalance == AccountPicker.SelectedItem).nuban;
                var card = GlobalStaticFields.Customer.MyCards.Where(x => x.MaskedPan == CardPicker.SelectedItem).SingleOrDefault().CardPan;
                var debitCard = CardToDebitTxt.Text;
                var currency = CardCurrencyPicker.SelectedItem;
                var expiry = ExpiryTxt.Text.Replace("/", "");
                var cvv = CVVTxt.Text;
                var pin = PinTxt.Text;
                var amount = AmountTxt.Text;


                //if (string.IsNullOrWhiteSpace(DestinationCardPicker.SelectedItem))
                //{
                //    MessageDialog.Show("Fund Card", "Kindly select whose card to fund.", DialogType.Error, "OK", null);
                //    return;
                //}

                if (string.IsNullOrWhiteSpace(MethodPicker.SelectedItem))
                {
                    MessageDialog.Show("Fund Card", "Kindly select funding method.", DialogType.Error, "OK", null);
                    return;
                }

                if (string.IsNullOrWhiteSpace(card))
                {
                    MessageDialog.Show("Fund Card", "Kindly select the card to fund.", DialogType.Error, "OK", null);
                    return;
                }

                if (string.IsNullOrWhiteSpace(amount))
                {
                    MessageDialog.Show("Fund Card", "Kindly enter the amount to fund.", DialogType.Error, "OK", null);
                    return;
                }

                if (MethodPicker.SelectedItem == "From Card")
                {
                    if (string.IsNullOrWhiteSpace(debitCard))
                    {
                        MessageDialog.Show("Fund Card", "Kindly enter the card to debit.", DialogType.Error, "OK", null);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(expiry))
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

                    if (string.IsNullOrWhiteSpace(currency))
                    {
                        MessageDialog.Show("Fund Card", "Kindly enter the card's currency.", DialogType.Error, "OK", null);
                        return;
                    }

                    cardModel = new FundWithCardModel()
                    {
                        pan = debitCard,
                        cvv = cvv,
                        pin = pin,
                        expiry_date = expiry,
                        currency = currency,
                        CreditAccount = card,
                        amount = amount,
                        Translocation = "0,0",
                        customerId = GlobalStaticFields.Customer.CustomerId,
                        RequestType = 211,
                        Referenceid = "5991" + DateTime.Now.Ticks.ToString(),
                        remark = " "
                    };

                    if (GlobalStaticFields.Customer.IsTPin)
                    {
                        var FundAccountVerifyTPIN = new PopUps.VerifyPinPage("Confirmation", amount, "NGN", null);
                        FundAccountVerifyTPIN.Validated += FundAccountVerifyTPIN_Validated;
                        await Navigation.PushAsync(FundAccountVerifyTPIN);
                    }
                    else
                    {

                        FundCardWithCard();
                    }

                }
                else if (MethodPicker.SelectedItem == "From Account")
                {
                    var accountToDebit = GlobalStaticFields.Customer.ListOfAllAccounts.Where(x => x.nuban == AccountPicker.SelectedItem).SingleOrDefault();
                    var account = accountToDebit.nuban;

                    if (string.IsNullOrWhiteSpace(account))
                    {
                        MessageDialog.Show("Fund Card", "Kindly select account to debit.", DialogType.Error, "OK", null);
                        return;
                    }


                    if (accountToDebit.accountType.ToLower().Contains("wallet"))
                    {
                        walletdModel = new FundWithWalletModel()
                        {
                            Referenceid = "5991" + DateTime.Now.Ticks.ToString(),
                            toPAN = card,
                            Amt = amount,
                            WalletfromAcct = account,
                            Narrations = " "
                        };

                        if (GlobalStaticFields.Customer.IsTPin)
                        {
                            var FundAccountVerifyTPINs = new PopUps.VerifyPinPage("Confirmation", amount, "NGN", null);
                            FundAccountVerifyTPINs.Validated += FundAccountVerifyTPINs_Validated; ;
                            await Navigation.PushAsync(FundAccountVerifyTPINs);
                        }
                        else
                        {

                            FundCardWithWallet();
                        }
                        
                    }
                    else
                    {
                        accountModel = new FundWithAccountModel()
                        {
                            Referenceid = "5991" + DateTime.Now.Ticks.ToString(),
                            PAN = card,
                            Amount = amount,
                            Account = account,
                            RequestType = 213,
                            Narration = " ",
                            Translocation = "0,0"

                        };
                        if (GlobalStaticFields.Customer.IsTPin)
                        {
                            var FundAccountVerifyTPINeS = new PopUps.VerifyPinPage("Confirmation", amount, "NGN", null);
                            FundAccountVerifyTPINeS.Validated += FundAccountVerifyTPINeS_Validated; ;
                            await Navigation.PushAsync(FundAccountVerifyTPINeS);
                        }
                        else
                        {

                            FundCardWithAccount();
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                await BusinessLogic.Log(ex.ToString(), "Exception on Fund Card", "", "", "", "FundCardPage");
            }
        }

        private void FundAccountVerifyTPINeS_Validated(object sender, bool e)
        {
            FundCardWithAccount();
        }

        private void FundAccountVerifyTPINs_Validated(object sender, bool e)
        {
            FundCardWithWallet();
        }

        private void FundAccountVerifyTPIN_Validated(object sender, bool e)
        {
            FundCardWithCard();
        }
    }
}