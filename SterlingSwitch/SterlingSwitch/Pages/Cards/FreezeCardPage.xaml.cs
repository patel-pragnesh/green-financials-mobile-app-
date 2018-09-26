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

namespace SterlingSwitch.Pages.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FreezeCardPage : SwitchMasterPage
    {
		public FreezeCardPage (MyCards card)
		{
			InitializeComponent ();
            MyCard = card;

            Task.Run(() => GetEnableChannels(card.CardPan));
        }

        MyCards MyCard;
        ApiRequest apirequest = new ApiRequest();
        string BaseUrl = URLConstants.SwitchApiLiveBaseUrl;

        public async void GetEnableChannels(string pan)
        {
            try
            {
                string endpoint = "Switch/GetEnabledChannels";
                string url = BaseUrl + endpoint;

                var cardParam = new Dictionary<string, string>();
                cardParam.Add("PAN", pan);

                var result = await apirequest.Post(cardParam, "", BaseUrl, endpoint, "FreezeCardPage");

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resTxt = await result.Content.ReadAsStringAsync();

                    //if (resTxt.ToUpper().Contains("ATM"))
                    //    AtmCheckBx.IsToggled = true;
                    //else
                    //    AtmCheckBx.IsToggled = false;

                    if (resTxt.ToUpper().Contains("POS"))
                        PosCheckBx.IsToggled = true;
                    else
                        PosCheckBx.IsToggled = false;

                    if (resTxt.ToUpper().Contains("WEB"))
                        WebCheckBx.IsToggled = true;
                    else
                        WebCheckBx.IsToggled = false;

                    if (WebCheckBx.IsToggled && PosCheckBx.IsToggled)
                        AllCheckBx.IsToggled = true; 
                }
            }
            catch (Exception)
            {
                ;
            }

        }

        public async Task<string> CardOperationEnableChannels(string pan, int channelsid)
        {
            string resTxt = string.Empty;

            var pd = await ProgressDialog.Show("Processing...please wait!");

            try
            {
                string endpoint = "Switch/CardOperationEnableChannel";
                string url = BaseUrl + endpoint;


                var cardParam = new Dictionary<string, string>();

                cardParam.Add("PAN", pan);
                cardParam.Add("Channelsid", channelsid.ToString());

                var result = await apirequest.Post(cardParam, "", BaseUrl, endpoint, "FreezeCardPage");

                resTxt = await result.Content.ReadAsStringAsync();
            }
            catch (WebException ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Data Connection Error", "Check your data connection and try again.", DialogType.Error, "OK", null);
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
            }

            await pd.Dismiss();
            return resTxt;
        }

        public async Task<string> CardOperationDissableChannels(string Pan, int channelsid)
        {
            string resTxt = string.Empty;

            var pd = await ProgressDialog.Show("Processing...please wait!");

            try
            {
                string endpoint = "Switch/CardOperationDissableChannels";
                string url = BaseUrl + endpoint;


                var cardParam = new Dictionary<string, string>();

                cardParam.Add("PAN", Pan);
                cardParam.Add("Channelsid", channelsid.ToString());

                var result = await apirequest.Post(cardParam, "", BaseUrl, endpoint, "FreezeCardPage");

                resTxt = await result.Content.ReadAsStringAsync();

                return resTxt;
            }
            catch (WebException)
            {
                await pd.Dismiss();
                MessageDialog.Show("Data Connection Error", "Check your data connection and try again.", DialogType.Error, "OK", null);
            }
            catch (Exception)
            {
                await pd.Dismiss();
                return resTxt;
            }

            await pd.Dismiss();

            return resTxt;
        }


        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }

        
        private void AllCheckBx_Toggled(object sender, ToggledEventArgs e)
        {
            WebCheckBx.IsToggled = true;
            PosCheckBx.IsToggled = true;
        }

        private async void DoneBtn_Clicked(object sender, EventArgs e)
        {
            int operationCount = 0;
            int successCount = 0;

            if (PosCheckBx.IsToggled == true)
            {
                operationCount++;
                var txt = await CardOperationEnableChannels(MyCard.CardPan, 1);

                if (txt.ToString().ToUpper().Contains("SUCCESS"))
                    successCount++;
            }
            else
            {
                operationCount++;
                var txt = await CardOperationDissableChannels(MyCard.CardPan, 1);

                if (txt.ToString().ToUpper().Contains("SUCCESS"))
                    successCount++;
            }

            if (WebCheckBx.IsToggled == true)
            {
                operationCount++;
                var txt = await CardOperationEnableChannels(MyCard.CardPan, 2);

                if (txt.ToString().ToUpper().Contains("SUCCESS"))
                    successCount++;
            }
            else
            {
                operationCount++;
                var txt = await CardOperationDissableChannels(MyCard.CardPan, 2);

                if (txt.ToString().ToUpper().Contains("SUCCESS"))
                    successCount++;
            }

            if (operationCount == successCount)
            {
                MessageDialog.Show("Freeze Card", "Operation was successful.", DialogType.Success, "OK",
                    () =>
                    {
                        Device.BeginInvokeOnMainThread(async () => { await Navigation.PopAsync(true); });
                    });
            }
            else
                MessageDialog.Show("Freeze Card", "Unable to complete this operation, please try again", DialogType.Success, "OK", null);



        }
    }
}