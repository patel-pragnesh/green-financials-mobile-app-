using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.ForgotPin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPasswordPage : SwitchMasterPage
    {
       
        public LoginResponse UserInfo { get; set; }
        static ApiRequest httpService = new ApiRequest();

        public ForgotPasswordPage ()
		{
			InitializeComponent ();
            this.BindingContext = this;
		}

        private async void btnContinue_Clicked(object sender, System.EventArgs e)
        {
           
            if (BusinessLogic.IsConnectionOK() == true)
            {
                // proceed with process
                if (IsEntryValid() == true)
                {
                    var pd = await ProgressDialog.Show("Sending Request. Please Wait...");

                    var response = await _getUserInfo(txtUsername.Text);
                    if (response.Status == true)
                    {
                        
                        var request = new OTPRequestMobile();
                        request.email = response.UserEmail;
                        request.mobile = response.PhoneNumber;

                        var otpResponse = await _getCustomerotp(request);
                        await pd.Dismiss();
                        if (otpResponse.response == "00")
                        {
                            MessageDialog.Show("INFO","An OTP has been sent to your registered phone number and email", DialogType.Info, "OK",null);
                            await Navigation.PushAsync(new  SetNewPinPage(response.PhoneNumber, response.UserEmail));
                        }
                        else
                             MessageDialog.Show("Error", "Sorry, We were unable to send you an OTP at he moment, kindly try again later.", PopUps.DialogType.Error, "Ok", null);
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Error", "Sorry, We were unable to find your username. Kindly verify that the username exists and try again.", PopUps.DialogType.Error, "Ok", null);
                    }
                   
                }    
            }
            else
            {               
                MessageDialog.Show("OOPS", "Sorry, it appears you do not have internet connectivity on your device. Kindly reconnect and try again", DialogType.Error, "DISMISS", null);               
                return;
            }
            // perform next opertion
        }

        private async Task<LoginResponse> _getUserInfo(string userEmail)
        {
            
            try
            {
                var userObj = new preferencesCustomer()
                {
                    UserID = userEmail
                };
                var loginResponse = await httpService.Post<preferencesCustomer>(userObj, "", URLConstants.SwitchApiBaseUrl, "Switch/GetUserInfo", "ForgotPassword");
                if (loginResponse.IsSuccessStatusCode)
                {
                    var response = await loginResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LoginResponse>(response);
                    UserInfo = result;
                    return result;
                }
            }
            catch (Exception ex)
            {
                // log your exception
                await BusinessLogic.Log(ex.ToString(), "Exception changing PIN", string.Empty, string.Empty, string.Empty, string.Empty);

            }
            return null;
          
        }

        private async Task<StatusMessage> _getCustomerotp(OTPRequestMobile request)
        {
            request.Referenceid = Utilities.GenerateReferenceId();
            request.Translocation = "23499,8909090";
            request.RequestType = 142; 

            var otpResponse = await httpService.Post<OTPRequestMobile>(request, "", URLConstants.SwitchApiLiveBaseUrl, "Spay/OTPRequestMobile", "ForgotPassword");
            if (otpResponse.IsSuccessStatusCode)
            {
                var response = await otpResponse.Content.ReadAsStringAsync();
                var Message = JsonConvert.DeserializeObject<StatusMessage>(response);
                return Message;
            }
            return null;
        }

        private bool IsEntryValid()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageDialog.Show("OOPS", "Please enter your Username/Email address to proceed", DialogType.Error, "DISMISS", null);
                return false;
            }
            else
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(txtUsername.Text);
                if (!match.Success)
                {
                    MessageDialog.Show("OOPS", "Sorry, it appears that the email address you entered is not valid. Kindl review and try again. Please verify and try again.", DialogType.Error, "DISMISS", null);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}