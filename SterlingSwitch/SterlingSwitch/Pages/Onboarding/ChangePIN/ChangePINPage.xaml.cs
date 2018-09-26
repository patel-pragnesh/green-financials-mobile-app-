using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Onboarding.Login;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.ChangePIN
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePINPage : SwitchMasterPage
    {
        ICryptoService _crypto = DependencyService.Get<ICryptoService>();
        static ApiRequest httpService = new ApiRequest();
        public ChangePINPage ()
		{
			InitializeComponent ();
		} 

        private async void btnChangePIN_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOldPIN.Text) ||
                string.IsNullOrEmpty(txtNewPIN.Text) ||
                string.IsNullOrEmpty(txtConfirmPIN.Text))
            {
                MessageDialog.Show("OOPS", "Sorry, All fields are required", PopUps.DialogType.Error, "OK", null);
                return;
            }

            if (txtNewPIN.Text != txtConfirmPIN.Text)
            {
                MessageDialog.Show("OOPS", "Sorry, New PIN and confirm PIN do not match", PopUps.DialogType.Error, "OK", null);
                return;
            }

            try
            {
                    
                    var pd = await ProgressDialog.Show("Processing. Please Wait...");
                    var oldpwd = _crypto.Encrypt(txtOldPIN.Text);
                    var newPwd = _crypto.Encrypt(txtNewPIN.Text);

                    var user = new UserExist()
                    {
                        Username = GlobalStaticFields.Customer.Email,
                        Password = oldpwd
                    };
                    
                    // 1. Check if user exists, using the above model
                    var status = await httpService.Post<UserExist>(user, "", URLConstants.SwitchApiBaseUrl, "Switch/GetUserStatus", "Change PIN page");
                    if (status.IsSuccessStatusCode)
                    {
                        var content = await status.Content.ReadAsStringAsync();
                        var content2 = JsonConvert.DeserializeObject<string>(content);
                        if (content2 == "true")
                        {
                            // 2. perform the PIN change if user truly exist
                            var changePassword = new ChangePassword()
                            {
                                UserID = GlobalStaticFields.Customer.Email,
                                Password = oldpwd,
                                NewPassword = newPwd,
                                TPIN = newPwd
                            };

                            var response = await httpService.Post<ChangePassword>(changePassword, "", URLConstants.SwitchApiBaseUrl, "Switch/ChangePassword", "Change PIn Page");
                            if (response.IsSuccessStatusCode)
                            {
                                var resp = await response.Content.ReadAsStringAsync();
                                var resp2 = JsonConvert.DeserializeObject<string>(resp);
                                if (resp2 == "1")
                                {
                                    await pd.Dismiss();
                                    MessageDialog.Show("SUCCESS", "Your PIN was successfully changed.", PopUps.DialogType.Success, "OK", null);
                                    await Navigation.PushAsync(new UnProfiledLoginPage());
                                }
                                else
                                {
                                    await pd.Dismiss();
                                    MessageDialog.Show("OOPS", "Sorry, we were unable to change your PIN. Kindly try again later", PopUps.DialogType.Error, "OK", null);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            await pd.Dismiss();
                            MessageDialog.Show("OOPS", "Sorry, we could not recognize your old pin. Kindly verify and try again", PopUps.DialogType.Error, "OK", null);
                            return;
                        }
                    }
                    await pd.Dismiss();

              
            }
            catch(Exception ex) {

                //await pd.Dismiss();
            }
        }

        public bool IsValidated()
        {
            if (string.IsNullOrEmpty(txtOldPIN.Text) ||
                string.IsNullOrEmpty(txtNewPIN.Text) ||
                string.IsNullOrEmpty(txtConfirmPIN.Text))
            {
                MessageDialog.Show("OOPS", "Sorry, All fields are required", PopUps.DialogType.Error, "OK", null);
                return false;
            }
            else if (txtNewPIN.Text != txtConfirmPIN.Text)
            {
                MessageDialog.Show("OOPS", "Sorry, New PIN and confirm PIN do not match", PopUps.DialogType.Error, "OK", null);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}