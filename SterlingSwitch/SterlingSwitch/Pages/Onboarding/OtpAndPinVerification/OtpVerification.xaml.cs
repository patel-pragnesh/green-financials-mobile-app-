using Newtonsoft.Json;
using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion;
using SterlingSwitch.Pages.Onboarding.Services;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.OtpAndPinVerification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OtpVerification : SwitchMasterPage
    {
        OnboardingViewModel _vm;        
		public OtpVerification (OnboardingViewModel model)
		{
            _vm = model;
            
			InitializeComponent ();
		}

         public string Pin = string.Empty;

        private void GetInput(string value)
        {

            if (Pin.Length < 6)
            {
                Pin += value;
                UpdateDots(Pin.Length);
                if (Pin.Length == 6)
                {
                    DoOTPVerification();
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

        private async void DoOTPVerification()
        {
            var pd = await ProgressDialog.Show("Processing Request..... Please wait.");
            try
            {
                Random r = new Random();
                r.Next(1000, 9999);
                var otpRequest = new ValOtpRequestMobile()
                {
                    mobile = _vm.WalletPhone,
                    otp = Pin,
                    Referenceid = "00055" + DateTime.Now.ToString("yymmddHHmmss") + r.Next().ToString(),
                    RequestType = "143",
                    Translocation = GlobalStaticFields.GetUserLocation
                };
                var response = await OnBoardingService.ValidateOTP(otpRequest);
                
                if (!string.IsNullOrEmpty(response))
                {
                    var data = JsonConvert.DeserializeObject<StatusMessage>(response);
                    if(data.response == "00")
                    {                       
                       
                        await pd.Dismiss();

                        MessageDialog.Show("SUCCESS", "OTP Verification and wallet creation was successful.", DialogType.Success, "OK", null);
                        // before creating the pin, lets set some security questions
                        // Navigation.PushAsync(new AccountPinCreation(_vm)); 
                        Navigation.PushAsync(new SecurityQuestionPage(_vm));             
                    }                    
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("OOPS", "OTP Verification was not successful. Kindly verify and try again", DialogType.Error, "OK", null);
                    return;
                }
                await pd.Dismiss();
            }
            catch(Exception ex)
            {
                await pd.Dismiss();
                string log = ex.Message;
            }
        }

        private async Task<bool> DoWalletCreation()
        {
            bool isWalletCreated = false;
            Random rnd = new Random();
            rnd.Next(1000, 9999);
            var walletAccount = new CustomerWalletAccount()
            {
                Referenceid = "00055" + DateTime.Now.ToString("yymmddHHmmss") + rnd.Next().ToString(),
                RequestType = "117",
                BIRTHDATE = _vm.DateOfBirth.ToString(),
                FIRSTNAME = _vm.Firstname,
                LASTNAME = _vm.Lastname,
                GENDER = _vm.Gender.Substring(0, 1),              
                EMAIL = _vm.Email,
                MARITAL_STATUS = "",
                MOBILE = _vm.PhoneNumber,
                CUST_STATUS = "1",
                CUST_TYPE = "1",
                CATEGORYCODE = "10001",
                RESIDENCE = "NG",
                NATIONALITY = "Nigerian",
                TARGET = "1111",
                SECTOR = 1,
                TITLE = "",
                ADDR_LINE1 = "",
                ADDR_LINE2 = ""
            };

            var response = await OnBoardingService.CreateWalletAccount(walletAccount);
            if (!string.IsNullOrEmpty(response))
            {
                var data = JsonConvert.DeserializeObject<CreateWalletResponse>(response);
                if (data.response == "00")
                {
                    isWalletCreated = true;
                }
                else
                    isWalletCreated = false;
            }
            return isWalletCreated;
        }
        private void UpdateDots(int length)
        {
            Color fillColor = Color.Black;
            Color noColor = Color.Transparent;
            switch (length)
            {
                case 0:
                    CellOne.Text = string.Empty;
                    CellTwo.Text = string.Empty;
                    CellThree.Text = string.Empty;
                    CellFour.Text = string.Empty;
                    CellFive.Text = string.Empty;
                    CellSix.Text = string.Empty;
                    break;
                case 1:
                    CellOne.Text = Pin[0].ToString();
                    CellTwo.Text = string.Empty;
                    CellThree.Text = string.Empty;
                    CellFour.Text = string.Empty;
                    CellFive.Text = string.Empty;
                    CellSix.Text = string.Empty;
                    break;
                case 2:
                    CellOne.Text = Pin[0].ToString();
                    CellTwo.Text = Pin[1].ToString();
                    CellThree.Text = string.Empty;
                    CellFour.Text = string.Empty;
                    CellFive.Text = string.Empty;
                    CellSix.Text = string.Empty;
                    break;
                case 3:
                    CellOne.Text=Pin[0].ToString();
                    CellTwo.Text = Pin[1].ToString();
                    CellThree.Text = Pin[2].ToString();
                    CellFour.Text = string.Empty;
                    CellFive.Text = string.Empty;
                    break;
                case 4:
                    CellOne.Text = Pin[0].ToString();
                    CellTwo.Text = Pin[1].ToString();
                    CellThree.Text = Pin[2].ToString();
                    CellFour.Text = Pin[3].ToString();
                    CellFive.Text = string.Empty;
                    CellSix.Text = string.Empty;
                    break;

                case 5:
                    CellOne.Text = Pin[0].ToString();
                    CellTwo.Text = Pin[1].ToString();
                    CellThree.Text = Pin[2].ToString();
                    CellFour.Text = Pin[3].ToString();
                    CellFive.Text = Pin[4].ToString();
                    CellSix.Text = string.Empty;
                    break;

                case 6:
                    CellOne.Text = Pin[0].ToString();
                    CellTwo.Text = Pin[1].ToString();
                    CellThree.Text = Pin[2].ToString();
                    CellFour.Text = Pin[3].ToString();
                    CellFive.Text = Pin[4].ToString();
                    CellSix.Text = Pin[5].ToString();
                    break;
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

        private void cancelTapped_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}