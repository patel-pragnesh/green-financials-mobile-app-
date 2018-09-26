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
	public partial class CardLimitPage : SwitchMasterPage
    {
		public CardLimitPage (MyCards card)
		{
			InitializeComponent ();
            this.card = card;
            GetCardLimits();
        }

        MyCards card;

        string BaseURL = URLConstants.SwitchApiLiveBaseUrl;
       
        private async void GetCardLimits()
        {
            var pd = await ProgressDialog.Show("Getting card limits...please wait!");

            string endpoint = "Switch/GetCardLimits";
            string url = BaseURL + endpoint;


            CardLimitReq clr = new CardLimitReq()
            {
                pan = card.CardPan,
                sequenceNumber = card.SequenceNumber,
                CardProgram = card.CardProgram,
                userEmail = GlobalStaticFields.Customer.Email
            };
            

            try
            {
                var apirequest = new ApiRequest();
                var response = await apirequest.Post(clr, "", BaseURL, endpoint, "CardLimitPage");
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    jsonString = jsonString.TrimStart('"');
                    jsonString = jsonString.TrimEnd('"');

                    var result = jsonString.Split('|');

                    PurcahseLimitTxt.RightText = "NGN " + double.Parse(CheckNull(result[1])).ToString("N2");
                    CashWithdrawalLimitTxt.RightText = "NGN " + double.Parse(CheckNull(result[3])).ToString("N2");
                    PaymentLimitTxt.RightText = "NGN " + double.Parse(CheckNull(result[6])).ToString("N2");

                    ForeignLimitTxt.RightText = "NGN " + double.Parse(CheckNull(result[8])).ToString("N2");

                    await pd.Dismiss();
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Card Limit", "Error getting card limit, please try again later.", PopUps.DialogType.Error, "OK", null);
                    await BusinessLogic.Log(response.StatusCode.ToString(), "Response Code is not OK while getting card limit", url, "", response.ToString(), "CardLimitPage");
                }

            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Card Limit", "Error getting card limit, please try again later.", PopUps.DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Exception while getting card limits", url, "", "", "CardLimitPage");
            }
        }

        private string CheckNull(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? "0" : s;
        }

        private void LocalTapped(object sender, object e)
        {
            LocalView.IsVisible = !LocalView.IsVisible;
        }

        private void IntlTapped(object sender, object e)
        {
            InternationalView.IsVisible = !InternationalView.IsVisible;
        }
    }
}