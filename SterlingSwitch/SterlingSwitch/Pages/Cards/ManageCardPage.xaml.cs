using SterlingSwitch.Pages.Cards.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManageCardPage : SwitchMasterPage
    {
		public ManageCardPage (MyCards card)
		{
			InitializeComponent ();

            MyCard = card;
            GetCountries();
        }

        MyCards MyCard;
        
        private string AllCountries = string.Empty;
        ApiRequest apiRequest = new ApiRequest();
        string BaseUrl = URLConstants.SwitchApiLiveBaseUrl;

        public async Task<string> CardOperationEnableCountries(string pan, string country)
        {
            string resTxt = string.Empty;

            var pd = await ProgressDialog.Show("Processing...please wait!");

            try
            {
                string endpoint = "Switch/CardOperationEnableCountries";
                string url = BaseUrl + endpoint;


                var cardParam = new Dictionary<string, string>();
                cardParam.Add("PAN", pan);
                cardParam.Add("Country", country);

                var result = await apiRequest.Post(cardParam, "", BaseUrl, endpoint, "ManageCardPage");
                
                resTxt = await result.Content.ReadAsStringAsync();

                return resTxt;
            }
            catch (WebException ex)
            {
                var r = await DisplayAlert("Data Connection Error", "Check your data connection and tap Retry to try again.", "Retry", "Cancel");

                if (r == true)
                {
                    await CardOperationEnableCountries(pan, country);
                }
                else
                    return resTxt;
            }
            catch (Exception)
            {
                return resTxt;
            }

            await pd.Dismiss();
            return resTxt;
        }

        public async Task<string> CardOperationDissableCountries(string pan, string country)
        {
            string resTxt = string.Empty;

            var pd = await ProgressDialog.Show("Processing...please wait!");

            try
            {
                string endpoint = "Switch/CardOperationDissableCountries";
                string url = BaseUrl + endpoint;

                
                var cardParam = new Dictionary<string, string>();
                cardParam.Add("PAN", pan);
                cardParam.Add("Country", country);


                var result = await apiRequest.Post(cardParam, "", BaseUrl, endpoint, "ManageCardPage");

                resTxt = await result.Content.ReadAsStringAsync();

                return resTxt;
            }
            catch (WebException)
            {
                var r = await DisplayAlert("Data Connection Error", "Check your data connection and tap Retry to try again.", "Retry", "Cancel");

                if (r == true)
                {
                    await CardOperationDissableCountries(pan, country);
                }
                else
                    return resTxt;
            }
            catch (Exception)
            {
                return resTxt;
            }

            await pd.Dismiss();
            return resTxt;
        }

        private async void GetCountries()
        {
            await Task.Run(() =>    // by putting this Task.Run only the Activity Indicator is shown otherwise its not shown.  So we have added this.
            {
                var assembly = this.GetType().GetTypeInfo().Assembly;

                Stream stream = assembly.GetManifestResourceStream("SterlingSwitch.Resources.CountryInfo.xml");
                string text = "";
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                XDocument xdoc = XDocument.Parse(text);


                var popList = (from item in xdoc.Descendants("country").Where(i => i.Attribute("cca2") != null && i.Attribute("cca2").Value.Trim() != string.Empty)
                               select new CheckItem()
                               {
                                   Name = item.Attribute("name").Value.Split(',').FirstOrDefault(),
                                   Code = item.Attribute("cca2").Value
                               }).OrderBy(x => x.Name).ToList();

                foreach (var item in popList)
                {
                    AllCountries += $"{item.Code},";
                }

            });
        }

        private void ActivateBtn_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ActivateCardPage());
        }

        private void ViewDetailsBtn_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CardDetailsPage(MyCard), true);
        }

        private async void EnableSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (EnableSwitch.IsToggled == true)
            {
                var enableCardFXPage = new EnableCardFXPage();

                enableCardFXPage.CardEnabled += async (s, t) =>
                {
                    if (t == true)
                    {
                        var result = await CardOperationEnableCountries(MyCard.CardPan, AllCountries);
                    }

                };

                await Navigation.PushModalAsync(enableCardFXPage, true);
            }
            else
            {
                await CardOperationDissableCountries(MyCard.CardPan, AllCountries); ;
            }
        }

        private void ViewLimitBtn_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CardLimitPage(MyCard), true);
        }
    }
}