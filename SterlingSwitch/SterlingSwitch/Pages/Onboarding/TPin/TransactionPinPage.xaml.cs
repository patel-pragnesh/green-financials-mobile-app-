using FormsControls.Base;
using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.TPin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionPinPage : SwitchMasterPage, INotifyPropertyChanged
    {
        ICryptoService _crypto = DependencyService.Get<ICryptoService>();
        public string TPin { get; set; }
        public string ConfirmTPin { get; set; }
        public string LoginPin { get; set; }
        public bool IsTPinEnabled { get; set; } = GlobalStaticFields.Customer.IsTPin;
        public bool EnableStackTPIN { get; set; }
        static ApiRequest httpService = new ApiRequest();

        public TransactionPinPage ()
		{
            this.BindingContext = this;
            InitializeComponent ();
            SetStackStatus();
		}


        void SetStackStatus()
        {
            if (GlobalStaticFields.TPINToSet)
                stkDisableTPIN.IsVisible = true;
            else if (GlobalStaticFields.resetPin == true)
                stkDisableTPIN.IsVisible = false;
           
        }
        private async void btnSetTPIN_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                LoginPin = txtLoginPin.Text;
                ConfirmTPin = txtConfirmTPin.Text;
                TPin = txtTPin.Text;

                if (BusinessLogic.IsConnectionOK() == true)
                {
                    if (GlobalStaticFields.TPINToSet)
                    {
                        if (!string.IsNullOrEmpty(TPin) && !string.IsNullOrEmpty(ConfirmTPin) && !string.IsNullOrEmpty(LoginPin))
                        {
                            if (TPin != ConfirmTPin)
                            {
                                MessageDialog.Show("OOPS", "Sorry TPIN and Confirm TPIN do not match. Kindly verify and try again", PopUps.DialogType.Error, "OK", null);
                                return;
                            }
                            var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
                            var tpin = _crypto.Encrypt(TPin);
                            var password = _crypto.Encrypt(LoginPin);
                            var model = new TPinModel()
                            {
                                TPIN = tpin,
                                Password = "",
                                NewPassword = "",
                                UserID = GlobalStaticFields.Customer.Email
                            };

                            var tpinResponse = await httpService.Post<TPinModel>(model, "", URLConstants.SwitchApiBaseUrl, "Switch/SetTPIN", "UnProfiledLoginPage");
                            await pd.Dismiss();
                            if (tpinResponse.IsSuccessStatusCode)
                            {
                                var success = await tpinResponse.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(success))
                                {
                                    var resp = JsonConvert.DeserializeObject<string>(success);
                                    if (resp == "1")
                                    {                                    // display success message
                                        MessageDialog.Show("SUCCESS", "Transaction PIN was successfully created.", PopUps.DialogType.Success, "OK", null);
                                        await Navigation.PushAsync(new More.MorePage());
                                        // navigate out
                                    }
                                    else
                                    {
                                        MessageDialog.Show("OOPS", "Sorry We are unable to process your request at the moment. Kindly try again later ", PopUps.DialogType.Error, "OK", null);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(TPin))
                            {
                                MessageDialog.Show("OOPS", "Sorry TPIN field is Required", PopUps.DialogType.Error, "OK", null);
                                return;
                            }
                            if (string.IsNullOrEmpty(ConfirmTPin))
                            {
                                MessageDialog.Show("OOPS", "Sorry, Confirm TPIN field is required", PopUps.DialogType.Error, "OK", null);
                                return;
                            }
                            if (string.IsNullOrEmpty(LoginPin))
                            {
                                MessageDialog.Show("OOPS", "Sorry, Login PIN is required to authorize this process", PopUps.DialogType.Error, "OK", null);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(LoginPin))
                        {
                            var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
                            var password = _crypto.Encrypt(LoginPin);
                            var model = new TPinModel()
                            {
                                TPIN = "",
                                UserID = GlobalStaticFields.Customer.Email,
                                Password = "",
                                NewPassword = ""
                            };
                            var tpinResponse = await httpService.Post<TPinModel>(model, "", URLConstants.SwitchApiBaseUrl, "Switch/ResetTPin", "UnProfiledLoginPage");
                            await pd.Dismiss();
                            if (tpinResponse.IsSuccessStatusCode)
                            {
                                var success = await tpinResponse.Content.ReadAsStringAsync();
                                if (!string.IsNullOrEmpty(success))
                                {
                                    var resp = JsonConvert.DeserializeObject<string>(success);
                                    if (resp == "1")
                                    {                                    // display success message
                                        MessageDialog.Show("SUCCESS", "You have successfully disabled your Transaction PIN", PopUps.DialogType.Success, "OK", null);
                                        Application.Current.MainPage = new AnimationNavigationPage(new Dashboard.Dashboard());
                                        // navigate out
                                    }
                                    else
                                    {
                                        // display an unsuccessful message
                                        MessageDialog.Show("OOPS", "Sorry We are unable to process your request at the moment. Kindly try again later", PopUps.DialogType.Error, "OK", null);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageDialog.Show("OOPS", "Sorry, login PIN is required to authorize this process", PopUps.DialogType.Error, "OK", null);
                            return;
                        }
                    }
                }
                else
                {
                    MessageDialog.Show("OOPS", "Sorry, It appears you do not have internet connectivity on your phone. Kindly re-connect and try again", DialogType.Error, "OK", null);
                    return;
                }
            }
            catch(Exception ex) { }
        }

        private void toggleTPIN_Toggled(object sender, ToggledEventArgs e)
        {
            if (IsTPinEnabled == true)
            {
                IsTPinEnabled = true;
                EnableStackTPIN = true;
                stkDisableTPIN.IsVisible = true;
            }

            else
            {
                IsTPinEnabled = false;
                EnableStackTPIN = false;               
                stkDisableTPIN.IsVisible = false;

            }
                
        }
    }
}