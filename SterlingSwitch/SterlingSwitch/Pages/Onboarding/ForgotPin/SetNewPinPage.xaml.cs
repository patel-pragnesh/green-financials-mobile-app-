using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Onboarding.Login;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.ForgotPin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SetNewPinPage : SwitchMasterPage
    {
        static ApiRequest httpService = new ApiRequest();
        ICryptoService _crypto = DependencyService.Get<ICryptoService>();
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
		public SetNewPinPage (string phone, string email)
		{
			InitializeComponent ();
            PhoneNumber = phone;
            Email = email;
		}

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtOTP.Text))
            {
                MessageDialog.Show("OOPS", "Please enter the OTP that was sent to your registered email and mobile number", DialogType.Error, "OK", null);
                return;
            }

            if (string.IsNullOrEmpty(txtPin.Text))
            {
                MessageDialog.Show("OOPS", "Please enter your new PIN", DialogType.Error, "OK", null); 
                return;
            }

            if (string.IsNullOrEmpty(txtConfirmPin.Text))
            {   
                MessageDialog.Show("OOPS", "Please confirm your new PIN", DialogType.Error, "OK", null);
                return;
            }

            if (txtPin.Text != txtConfirmPin.Text)
            {
                MessageDialog.Show("OOPS", "Sorry, PIN and Confirm PIN do not match. Kindly review and try again", DialogType.Error, "OK", null); 
                return;
            }
            var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
            try
            {
                var request = new ValOtpRequestMobile();
                request.mobile = PhoneNumber;
                request.otp = txtOTP.Text;

                var result = await _validateOTP(request);
                await pd.Dismiss();
                if (result.response == "00")
                {
                    var response = await ChangePassword();
                    if (response)
                    {
                        MessageDialog.Show("SUCCESS", "Pin Change was Successful. Kindly login with your new PIN", DialogType.Success, "OK", null);
                        await Navigation.PushAsync(new UnProfiledLoginPage());
                    }
                    else
                    {
                        MessageDialog.Show("OOPS", "Sorry, We are unable to process your request at the moment. Kindly try again later.", DialogType.Error, "OK", null);
                        return;
                    }
                }
                else
                {
                    MessageDialog.Show("OOPS", "Sorry, We are unable to process your request at the moment. Kindly try again later.", DialogType.Error, "OK", null);
                    return;
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("OOPS", "Sorry, An error occured while processing your request. Kindly try again later.", DialogType.Error, "OK", null);
                string log = ex.Message;
            }
        }

        private async Task<StatusMessage> _validateOTP(ValOtpRequestMobile request)
        {

            try
            {
                request.Referenceid = Utilities.GenerateReferenceId();
                request.RequestType = "143";
                request.Translocation = "0,099";
                var loginResponse = await httpService.Post<ValOtpRequestMobile>(request, "", URLConstants.SwitchApiBaseUrl, "Spay/ValOTPRequestMobile", "SetNewPinPage");
              
                if (loginResponse.IsSuccessStatusCode)
                {
                    var response = await loginResponse.Content.ReadAsStringAsync();
                    var Message = JsonConvert.DeserializeObject<StatusMessage>(response);
                    return Message;
                }
                else
                    return new StatusMessage();
            }
            catch (Exception ui)
            {
                await BusinessLogic.Log(ui.ToString(), "Exception Validating OTP on Forgot Password Page", string.Empty, string.Empty, string.Empty, string.Empty);
                return new StatusMessage();
            }
       
        }

        private async Task<bool> ChangePassword()
        {
            try
            {   
                var newPin = _crypto.Encrypt(txtPin.Text);
                var request = new ResetPinRequest();
                request.Password = newPin;
                request.UserID = Email; 
                var response = await httpService.Post<ResetPinRequest>(request, "", URLConstants.SwitchApiBaseUrl, "Switch/ResetPin", "SetNewPinPage"); 
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;               
                 
            }
            catch (Exception CP)
            {
                await BusinessLogic.Log(CP.ToString(), "Exception changing PIN", string.Empty, string.Empty, string.Empty, string.Empty);
                return false;
            } 
        }
    }
}