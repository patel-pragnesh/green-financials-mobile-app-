using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Helper;
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

namespace SterlingSwitch.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerifyPinPage : SwitchMasterPage
    {
        ICryptoService _crypto = DependencyService.Get<ICryptoService>();
        public VerifyPinPage (string Title, string Amount, string Currency, string SubTitle = null)
		{
			InitializeComponent ();
            description.Text = $"Enter your transaction PIN to confirm \n this transaction";
            TitleTxt.Text = Title;
            AmountTxt.Text = Amount;
            CurrencyTxt.Text = Currency;
            SubTitleTxt.Text = SubTitle;
        }

        public event EventHandler<bool> Validated;
        string BaseURL = URLConstants.SwitchApiLiveBaseUrl;

        public string Pin = string.Empty;

        private void GetInput(string value)
        {

            if (Pin.Length < 4)
            {
                Pin += value;
                UpdateDots(Pin.Length);
                if (Pin.Length == 4)
                {                  
                    ValidatePin();
                }
            }
        }

        private void BackSpace()
        {
            if (!string.IsNullOrEmpty(Pin))
            {
                Pin = Pin.Remove(Pin.Length - 1);
                UpdateDots(Pin.Length);
            }

        }

        private void UpdateDots(int length)
        {
            Color fillColor = Color.Black;
            Color noColor = Color.Transparent;
            switch (length)
            {
                case 0:
                    CellOne.Source = "Circle.png";
                    CellTwo.Source = "Circle.png";
                    CellThree.Source = "Circle.png";
                    CellFour.Source = "Circle.png";
                    break;
                case 1:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.BackgroundColor = noColor;
                    CellThree.BackgroundColor = noColor;
                    CellFour.BackgroundColor = noColor;
                    break;
                case 2:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.Source = "FilledCircle.png";
                    CellThree.BackgroundColor = noColor;
                    CellFour.BackgroundColor = noColor;
                    break;
                case 3:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.Source = "FilledCircle.png";
                    CellThree.Source = "FilledCircle.png";
                    CellFour.BackgroundColor = noColor;
                    break;
                case 4:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.Source = "FilledCircle.png";
                    CellThree.Source = "FilledCircle.png";
                    CellFour.Source = "FilledCircle.png";
                    break;
            }
        }

        private async void ValidatePin()
        {
            var pd = await ProgressDialog.Show("Validating you Transaction PIN...please wait!");

            string endpoint = "Switch/TPin";
            string url = BaseURL + endpoint;

            var encrypted = _crypto.Encrypt(Pin);
            var tpinModel = new TransactionPinModel()
            {                
                UserID = GlobalStaticFields.Customer.Email,
                TransactionPIN = encrypted
            };

            try
            {
                var apirequest = new ApiRequest();
                var response = await apirequest.Post(tpinModel, "", BaseURL, endpoint, "TransactionPinPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await pd.Dismiss();
                    var jsonString = await response.Content.ReadAsStringAsync();

                    bool isValid = false;
                    bool.TryParse(jsonString, out isValid);

                    if (isValid)
                    {
                        Validated?.Invoke(this, isValid);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Pin = string.Empty;
                        UpdateDots(0);
                        MessageDialog.Show("Transaction Pin", "Wrong Transaction Pin, please verify and try again.", DialogType.Error, "OK", null);
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Transaction Pin", "Error validating Transaction Pin, please try again later.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(response.StatusCode.ToString(), "Response Code is not OK while getting validating Transaction Pin", url, "", response.ToString(), "TransactionPinPage");
                }

            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Transaction Pin", "Error validating Transaction Pin, please try again later.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Exception while getting card limits", url, "", "", "TransactionPinPage");
            }
        }

        private void InputClicked(object sender, object e)
        {
            var input = (PinItemView)sender;
            GetInput((string)input.CommandParameter);
        }

        private void BackSpaceClicked(object sender, object e)
        {
            BackSpace();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }
    }

    public class TransactionPinModel
    {
        public string UserID { get; set; }
        public string TransactionPIN { get; set; }
    }
}