using Newtonsoft.Json;
using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.Cards.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.Repository;
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
	public partial class ActivateCardPage : SwitchMasterPage
    {
        public ActivateCardPage()
        {
            InitializeComponent();
        }

        
        string BaseUrl = URLConstants.SwitchApiLiveBaseUrl;
        string Endpoint = "IBSIntegrator/IBSBridgeJSONForCard";

        MyCards MyCard;
        public string OTP { get; set; }
        public string Pin { get; set; }
        public string ConfirmPin { get; set; }


        List<MyCards> inactiveCards = new List<MyCards>();
        List<MyCards> myCards = new List<MyCards>();

        ApiRequest apiRequest = new ApiRequest();
        Customer customer = GlobalStaticFields.Customer;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetInactiveCards();
        }

        private async void GetInactiveCards()
        {
            var pd = await ProgressDialog.Show("Getting list of inactive cards, please wait.");

            try
            {
                ////Remove later=================
                //page.CustomerId = "268325";
                //page.lastName = "Alli";
                //page.firstName = "Oludayo";
                ////=============================

                foreach (var item in customer.UniqueCustomerIDs)
                {
                    var cardModel = new CardModel()
                    {
                        ReferenceID = Utilities.GenerateReferenceId(),
                        RequestType = "946",
                        CustomerID = customer.CustomerId
                    };

                    var request = await apiRequest.PostIBS<CardModel>(cardModel, "", BaseUrl, Endpoint, "ActivateCardPage");

                    var resTxt = await request.Content.ReadAsStringAsync();
                    resTxt = resTxt.JsonCleanUp();
                    resTxt = JsonConvert.DeserializeObject<CardObject>(resTxt).IBSResponse.ResponseText;

                    if (!string.IsNullOrWhiteSpace(resTxt) && resTxt.Length > 8)
                    {

                        if (resTxt.Split('|')[0].Length > 2)
                        {
                            string[] pans = resTxt.Split('~');

                            foreach (string pan in pans)
                            {
                                var cardInfo = pan.Split('|');

                                inactiveCards.Add(new MyCards()
                                {
                                    CardName = $"{customer.FirstName} {customer.LastName}",
                                    CardPan = cardInfo[0],
                                    MaskedPan = $"**** **** **** {cardInfo[0].Remove(0, cardInfo[0].Length - 4)}",
                                    ExpiryYear = cardInfo[1].Substring(0, 2),
                                    ExpiryMonth = cardInfo[1].Substring(2, 2),
                                    CVV = cardInfo[2],
                                    CardType = cardInfo[3]
                                });
                            }


                        }
                    } 
                }

                if (inactiveCards.Count <= 0)
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Activate Card", "No Inactive card(s) found.", DialogType.Info, "OK", null);
                    await Navigation.PopAsync();
                }
                else
                {
                    await pd.Dismiss();
                    List<string> cards = new List<string>();

                    foreach (var item in inactiveCards)
                        cards.Add(item.CardPan);

                    CardPicker.ItemsSource = cards;
                }

            }
            catch (System.Net.WebException ex)
            {
                await pd.Dismiss();
                await BusinessLogic.Log(ex.Message, "Web Exception....Data Connection Error", Endpoint, "", "", "ActivateCardPage");
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                await BusinessLogic.Log(ex.Message, "Exception....Any other card request error.", Endpoint, "", "", "ActivateCardPage");
                //NoCards();
            }
        }
        

        private ActivateCardModel CreateModel(string PAN, string seq_nr, string PIN, string expiryDate)
        {
            var model = new ActivateCardModel()
            {
                ReferenceID = Utilities.GenerateReferenceId(),
                RequestType = "937",
                PAN = PAN,
                PIN = PIN,
                seq_nr = seq_nr,
                expiryDate = expiryDate
            };
            
            return model;
        }

        public async void CallIBS()
        {
            var pd = await ProgressDialog.Show("Processing...please wait!");

            try
            {
                var model = CreateModel(MyCard.CardPan, MyCard.CVV, Pin, $"{MyCard.ExpiryYear}{MyCard.ExpiryMonth}");
                var result = await apiRequest.Post(model, "", BaseUrl, Endpoint, "ActivateCardPage");

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var resTxt = await result.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(resTxt))
                    {
                        await pd.Dismiss();
                        var resCode = JsonConvert.DeserializeObject<CardObject>(resTxt).IBSResponse.ResponseCode;
                        if (resCode == "00")
                            MessageDialog.Show("Activate Card", "Card Activated Successfully.", DialogType.Success, "OK", null);
                        else
                            MessageDialog.Show("Activate Card", "Unable to activate your card at this time, please try again later.", PopUps.DialogType.Info, "OK", null);
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Activate Card", "Unable to activate your card at this time, please try again later.", PopUps.DialogType.Info, "OK", null);
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Activate Card", "Unable to activate your card at this time, please try again later.", PopUps.DialogType.Info, "OK", null);
                await BusinessLogic.Log(ex.ToString(), " Error Calling IBS (Activate Card Page).", "", "", "", "ActivateCardPage");
            }

        }

        private void RequestBtn_Clicked(object sender, EventArgs e)
        {
            string validationMessage;
            Pin = SetPinTxt.Text;
            ConfirmPin = ConfirmPinTxt.Text;
            OTP = InputOTPTxt.Text;

            if (string.IsNullOrWhiteSpace(Pin))
            {
                validationMessage = "Please Enter Your Pin";
                MessageDialog.Show("Pin", validationMessage, DialogType.Info, "OK", null);
                return;
            }
            else if (string.IsNullOrWhiteSpace(ConfirmPin))
            {
                validationMessage = "Please Confirm Your Pin";
                MessageDialog.Show("Pin", validationMessage, DialogType.Info, "OK", null);
                return;
            }

            else if (string.IsNullOrWhiteSpace(OTP))
            {
                validationMessage = "Please Enter OTP Sent To Your Mobile Number";
                MessageDialog.Show("OTP", validationMessage, DialogType.Info, "OK", null);
                return;
            }

            else if (ConfirmPin != Pin)
            {
                validationMessage = "Your pin does not match.";
                MessageDialog.Show("Pin", validationMessage, DialogType.Info, "OK", null);
                return;
            }


            ValidateOTP(OTP);
        }

        private void CardPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CardPicker.SelectedItem == null)
                return;

            MyCard = myCards.Where(x => x.MaskedPan == CardPicker.SelectedItem.ToString()).SingleOrDefault();

            SendOTP();
        }
        private async void SendOTP()
        {
            var pd = await ProgressDialog.Show("Sending OTP to your registered phone number...please wait!");

            var result = await GlobalStaticFields.SendOTP().ConfigureAwait(false);

            if (result)
            {
                await pd.Dismiss();
                MessageDialog.Show("One Time Password", "OTP has been sent to your Phone and Email", DialogType.Info, "OK", null);
            }
            else
            {
                await pd.Dismiss();
                MessageDialog.Show("One Time Password", "Error sending OTP to your phone number.", DialogType.Info, "Retry", SendOTP, "Cancel", null);
            }
        }

        private async void ValidateOTP(string otp)
        {
            var pd = await ProgressDialog.Show("Validating OTP...please wait!");

            var result = await GlobalStaticFields.ValidateOTP(otp).ConfigureAwait(false);

            if (result)
            {
                await pd.Dismiss();
                CallIBS();
            }
            else
            {
                await pd.Dismiss();
                MessageDialog.Show("Invalid OTP", "You have entered an invalid OTP", DialogType.Info, "OK", null);
            }
        }
    }
}